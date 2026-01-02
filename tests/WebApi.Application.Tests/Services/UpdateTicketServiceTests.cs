using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Tickets.UpdateTicket;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Services;

public class UpdateTicketServiceTests : BaseServiceHandlerTest
{
    private readonly UpdateTicketService _service;
    private readonly Mock<ITicketRepository> _ticketRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly TicketNotificationFactory _notificationFactory;
    private readonly TicketBuilder _ticketBuilder;
    private readonly ProjectBuilder _projectBuilder;
    private readonly ProjectMemberBuilder _projectMemberBuilder;
    private readonly UserBuilder _userBuilder;

    public UpdateTicketServiceTests()
    {
        _ticketRepository = new Mock<ITicketRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _userRepository = new Mock<IUserRepository>();
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationFactory = new TicketNotificationFactory(Clock);
        _ticketBuilder = new TicketBuilder();
        _projectBuilder = new ProjectBuilder();
        _projectMemberBuilder = new ProjectMemberBuilder();
        _userBuilder = new UserBuilder();

        _service = new UpdateTicketService(
            _ticketRepository.Object,
            _projectRepository.Object,
            _userRepository.Object,
            _notificationRepository.Object,
            _notificationFactory,
            UnitOfWork.Object,
            DomainEventPublisher.Object,
            UserContext.Object,
            Clock
        );
    }

    [Fact]
    public async Task 正常系_UpdateTicketAsync_タイトルのみ更新()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var ticket = _ticketBuilder.WithProjectId(project.Id).Build();

        _ticketRepository
            .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var newTitle = "タイトル更新後";
        var command = new UpdateTicketCommand(
            projectId: ticket.ProjectId,
            ticketId: ticket.Id,
            title: Optional<string>.Of(newTitle),
            description: Optional<string>.None(),
            assigneeId: Optional<Guid?>.None(),
            startDate: Optional<DateOnly?>.None(),
            endDate: Optional<DateOnly?>.None(),
            status: Optional<string>.None(),
            completionCriterionOperations:
                Optional<IReadOnlyList<ICompletionCriterionOperationDto>>.None(),
            comment: Optional<string>.None(),
            notificationRecipientIds: new List<Guid>()
        );
        var result = await _service.UpdateTicketAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        ticket.Title.Value.Should().Be(newTitle);
        ticket.Description.Should().Be(ticket.Description);
        ticket.AssigneeId.Should().Be(ticket.AssigneeId);
        ticket.Schedule.Should().Be(ticket.Schedule);
        ticket.Status.Should().Be(ticket.Status);
        ticket.CompletionCriteria.Should().BeEquivalentTo(ticket.CompletionCriteria);
        ticket.Comments.Should().BeEquivalentTo(ticket.Comments);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_UpdateTicketAsync_Ticketが存在しない場合()
    {
        // Arrange
        _ticketRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Ticket?)null);

        // Act
        var command = new UpdateTicketCommand(
            projectId: Guid.NewGuid(),
            ticketId: Guid.NewGuid(),
            title: Optional<string>.None(),
            description: Optional<string>.None(),
            assigneeId: Optional<Guid?>.None(),
            startDate: Optional<DateOnly?>.None(),
            endDate: Optional<DateOnly?>.None(),
            status: Optional<string>.None(),
            completionCriterionOperations:
                Optional<IReadOnlyList<ICompletionCriterionOperationDto>>.None(),
            comment: Optional<string>.None(),
            notificationRecipientIds: new List<Guid>()
        );
        var act = async () => await _service.UpdateTicketAsync(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.TICKET_NOT_FOUND");
    }

    [Fact]
    public async Task 正常系_UpdateTicketAsync_Assign()
    {
        // Arrange
        var user = _userBuilder.Build();
        var member = _projectMemberBuilder.WithUserId(user.Id).Build();
        var project = _projectBuilder.WithMembers(member).Build();
        var ticket = _ticketBuilder.WithProjectId(project.Id).Build();

        _ticketRepository
            .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _userRepository
            .Setup(x => x.GetByIdAsync(member.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var command = new UpdateTicketCommand(
            projectId: ticket.ProjectId,
            ticketId: ticket.Id,
            title: Optional<string>.None(),
            description: Optional<string>.None(),
            assigneeId: Optional<Guid?>.Of(member.UserId),
            startDate: Optional<DateOnly?>.None(),
            endDate: Optional<DateOnly?>.None(),
            status: Optional<string>.None(),
            completionCriterionOperations:
                Optional<IReadOnlyList<ICompletionCriterionOperationDto>>.None(),
            comment: Optional<string>.None(),
            notificationRecipientIds: new List<Guid>()
        );
        var result = await _service.UpdateTicketAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        ticket.Title.Should().Be(ticket.Title);
        ticket.Description.Should().Be(ticket.Description);
        ticket.AssigneeId.Should().Be(member.UserId);
        ticket.Schedule.Should().Be(ticket.Schedule);
        ticket.Status.Should().Be(ticket.Status);
        ticket.CompletionCriteria.Should().BeEquivalentTo(ticket.CompletionCriteria);
        ticket.Comments.Should().BeEquivalentTo(ticket.Comments);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 正常系_UpdateTicketAsync_Unassign()
    {
        // Arrange
        var member = _projectMemberBuilder.Build();
        var project = _projectBuilder.WithMembers(member).Build();
        var ticket = _ticketBuilder
            .WithProjectId(project.Id)
            .WithAssigneeId(member.UserId)
            .Build();

        _ticketRepository
            .Setup(x => x.GetByIdAsync(ticket.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ticket);
        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var command = new UpdateTicketCommand(
            projectId: ticket.ProjectId,
            ticketId: ticket.Id,
            title: Optional<string>.None(),
            description: Optional<string>.None(),
            assigneeId: Optional<Guid?>.Of(null),
            startDate: Optional<DateOnly?>.None(),
            endDate: Optional<DateOnly?>.None(),
            status: Optional<string>.None(),
            completionCriterionOperations:
                Optional<IReadOnlyList<ICompletionCriterionOperationDto>>.None(),
            comment: Optional<string>.None(),
            notificationRecipientIds: new List<Guid>()
        );
        var result = await _service.UpdateTicketAsync(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        ticket.Title.Should().Be(ticket.Title);
        ticket.Description.Should().Be(ticket.Description);
        ticket.AssigneeId.Should().Be(null);
        ticket.Schedule.Should().Be(ticket.Schedule);
        ticket.Status.Should().Be(ticket.Status);
        ticket.CompletionCriteria.Should().BeEquivalentTo(ticket.CompletionCriteria);
        ticket.Comments.Should().BeEquivalentTo(ticket.Comments);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
