using FluentAssertions;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Tickets.CreateTicket;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Tickets;

public class CreateTicketHandlerTests : BaseCommandHandlerTest
{
    private readonly CreateTicketHandler _handler;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly TicketNotificationFactory _notificationFactory;
    private readonly TicketBuilder _ticketBuilder;
    private readonly ProjectBuilder _projectBuilder;

    public CreateTicketHandlerTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationFactory = new TicketNotificationFactory(Clock);
        _ticketBuilder = new TicketBuilder();
        _projectBuilder = new ProjectBuilder();

        _handler = new CreateTicketHandler(
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
        var project = _projectBuilder.WithMembers([
            ProjectMember.Create(Guid.NewGuid(), ProjectRole.Create(ProjectRole.RoleType.ProjectManager)),
            ProjectMember.Create(Guid.NewGuid(), ProjectRole.Create(ProjectRole.RoleType.Member))
        ]).Build();
        var ticket = _ticketBuilder.WithProjectId(project.Id).Build();

        Ticket? capturedTicket = null;
        _ticketRepository
            .Setup(x => x.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
            .Callback<Ticket, CancellationToken>((t, _) => capturedTicket = t)
            .Returns(Task.CompletedTask);

        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        List<Notification> capturedNotifications = [];
        _notificationRepository
            .Setup(x => x.AddRangeAsync(It.IsAny<List<Notification>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<Notification>, CancellationToken>((n, _) => capturedNotifications = n.ToList())
            .Returns(Task.CompletedTask);

        // Act
        var command = new CreateTicketCommand(
            ticket.ProjectId,
            ticket.Title.Value,
            ticket.AssigneeId,
            ticket.Deadline?.Value,
            ticket.CompletionCriteria,
            project.Members.Select(m => m.UserId).ToList()
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        capturedTicket.Should().NotBeNull();
        capturedTicket.ProjectId.Should().Be(ticket.ProjectId);
        capturedTicket.Title.Should().Be(ticket.Title);
        capturedTicket.AssigneeId.Should().Be(ticket.AssigneeId);
        capturedTicket.Deadline.Should().Be(ticket.Deadline);
        capturedTicket.Status.Should().Be(ticket.Status);
        capturedTicket.CompletionCriteria.Should().Be(ticket.CompletionCriteria);
        capturedTicket.CreatedBy.Should().Be(UserContext.Object.Id);
        capturedTicket.CreatedAt.Should().Be(Clock.Now);
        capturedTicket.UpdatedBy.Should().Be(UserContext.Object.Id);
        capturedTicket.UpdatedAt.Should().Be(Clock.Now);
        _ticketRepository.Verify(x => x.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Once);

        for (int i = 0; i < project.Members.Count; i++)
        {
            capturedNotifications[i].Should().NotBeNull();
            capturedNotifications[i].RecipientId.Should().Be(project.Members[i].UserId);
            capturedNotifications[i].Category.Value.Should().Be(NotificationCategory.Category.TicketCreated);
            capturedNotifications[i].SubjectId.Should().Be(result);
            capturedNotifications[i].Message.Should().Be($"チケット {ticket.Title} が作成されました。");
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
    public async Task 異常系_Handle_Projectが存在しない場合()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var ticket = _ticketBuilder.WithProjectId(project.Id).Build();

        Ticket? capturedTicket = null;
        _ticketRepository
            .Setup(x => x.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
            .Callback<Ticket, CancellationToken>((t, _) => capturedTicket = t)
            .Returns(Task.CompletedTask);

        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var command = new CreateTicketCommand(
            ticket.ProjectId,
            ticket.Title.Value,
            ticket.AssigneeId,
            ticket.Deadline?.Value,
            ticket.CompletionCriteria,
            project.Members.Select(m => m.UserId).ToList()
        );
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("PROJECT_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
