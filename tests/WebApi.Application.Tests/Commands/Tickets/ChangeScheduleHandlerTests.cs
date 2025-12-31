using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Tickets.ChangeStatus;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Tickets;

public class ChangeScheduleHandlerTests : BaseCommandHandlerTest
{
    private readonly ChangeScheduleHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly TicketNotificationFactory _notificationFactory;
    private readonly TicketBuilder _ticketBuilder;
    private readonly ProjectBuilder _projectBuilder;
    private readonly ProjectMemberBuilder _projectMemberBuilder;

    public ChangeScheduleHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationFactory = new TicketNotificationFactory(Clock);
        _ticketBuilder = new TicketBuilder();
        _projectBuilder = new ProjectBuilder();
        _projectMemberBuilder = new ProjectMemberBuilder();

        _handler = new ChangeScheduleHandler(
            _ticketRepository.Object,
            _projectRepository.Object,
            _notificationRepository.Object,
            _notificationFactory,
            UnitOfWork.Object,
            DomainEventPublisher.Object,
            UserContext.Object,
            Clock
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var member1 = _projectMemberBuilder
            .WithUserId(Guid.NewGuid())
            .WithRole(ProjectRole.RoleType.ProjectManager)
            .Build();
        var member2 = _projectMemberBuilder
            .WithUserId(Guid.NewGuid())
            .WithRole(ProjectRole.RoleType.Member)
            .Build();
        var project = _projectBuilder.Build();
        var ticket = _ticketBuilder.WithProjectId(project.Id).Build();

        _ticketRepository
            .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        _projectRepository
            .Setup(x => x.GetByIdAsync(ticket.ProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        List<Notification> capturedNotifications = [];
        _notificationRepository
            .Setup(x => x.AddRangeAsync(It.IsAny<List<Notification>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<Notification>, CancellationToken>((n, _) => capturedNotifications = n.ToList())
            .Returns(Task.CompletedTask);

        // Act
        var command = new ChangeScheduleCommand(
            ticket.ProjectId,
            ticket.Id,
            Clock.Today,
            Clock.Today.AddDays(7),
            project.Members.Select(m => m.UserId).ToList()
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        ticket.Schedule.StartDate.Should().Be(Clock.Today);
        ticket.Schedule.EndDate.Should().Be(Clock.Today.AddDays(7));
        ticket.AuditInfo.UpdatedBy.Should().Be(UserContext.Object.Id);
        ticket.AuditInfo.UpdatedAt.Should().Be(Clock.Now);

        for (int i = 0; i < project.Members.Count; i++)
        {
            capturedNotifications[i].Should().NotBeNull();
            capturedNotifications[i].RecipientId.Should().Be(project.Members.ElementAt(i).UserId);
            capturedNotifications[i].Category.Value.Should().Be(NotificationCategory.Category.TicketUpdated);
            capturedNotifications[i].SubjectId.Should().Be(ticket.Id);
            capturedNotifications[i].Message.Should().Be($"チケット {ticket.Title.Value} が更新されました。");
            capturedNotifications[i].IsRead.Should().Be(false);
        }
        _notificationRepository.Verify(x => x.AddRangeAsync(
            It.IsAny<List<Notification>>(), It.IsAny<CancellationToken>()),
            Times.Once);

        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Ticketが存在しない場合()
    {
        // Arrange
        var command = new ChangeScheduleCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Clock.Today,
            Clock.Today.AddDays(7),
            []
        );
        _ticketRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.TICKET_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task 異常系_Handle_Projectが存在しない場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();

        _ticketRepository
            .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);

        _projectRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var command = new ChangeScheduleCommand(
            ticket.ProjectId,
            ticket.Id,
            Clock.Today,
            Clock.Today.AddDays(7),
            []
        );
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.PROJECT_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
