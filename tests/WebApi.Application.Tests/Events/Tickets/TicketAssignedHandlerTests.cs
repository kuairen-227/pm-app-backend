using FluentAssertions;
using Moq;
using WebApi.Application.Common;
using WebApi.Application.Events.Tickets.MemberAssigned;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Events.Tickets;

public class TicketAssignedHandlerTests : BaseEventHandlerTest
{
    private MemberAssignedHandler _handler;
    private TicketNotificationFactory _notificationFactory;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly TicketBuilder _ticketBuilder;
    private readonly ProjectBuilder _projectBuilder;
    private readonly ProjectMemberBuilder _projectMemberBuilder;

    public TicketAssignedHandlerTests()
    {
        _notificationFactory = new TicketNotificationFactory(Clock);
        _notificationRepository = new Mock<INotificationRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _ticketBuilder = new TicketBuilder();
        _projectBuilder = new ProjectBuilder();
        _projectMemberBuilder = new ProjectMemberBuilder();

        _handler = new MemberAssignedHandler(
            _notificationFactory,
            _notificationRepository.Object,
            _projectRepository.Object,
            UnitOfWork.Object,
            UserContext.Object
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
        var project = _projectBuilder.WithMembers(member1, member2).Build();
        var ticket = _ticketBuilder
            .WithProjectId(project.Id)
            .WithAssigneeId(project.Members[1].UserId).Build();

        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        List<Notification> capturedNotifications = [];
        _notificationRepository
            .Setup(x => x.AddRangeAsync(It.IsAny<List<Notification>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<Notification>, CancellationToken>((n, _) => capturedNotifications = n.ToList())
            .Returns(Task.CompletedTask);

        // Act
        var notification = new MemberAssignedNotification(
            project.Members.Select(m => m.UserId).ToList(),
            ticket.Id,
            ticket.Title.Value,
            project.Members[0].UserId,
            "テスト 太郎",
            ticket.ProjectId
        );
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        capturedNotifications.Should().HaveCount(project.Members.Count());

        for (int i = 0; i < project.Members.Count; i++)
        {
            capturedNotifications[i].Should().NotBeNull();
            capturedNotifications[i].RecipientId.Should().Be(project.Members[i].UserId);
            capturedNotifications[i].Category.Value.Should().Be(NotificationCategory.Category.TicketMemberAssigned);
            capturedNotifications[i].SubjectId.Should().Be(ticket.Id);
            capturedNotifications[i].Message.Should().Be($"{notification.AssigneeName} が チケット {notification.TicketTitle} の担当になりました。");
            capturedNotifications[i].IsRead.Should().Be(false);
        }
        _notificationRepository.Verify(x => x.AddRangeAsync(
            It.IsAny<List<Notification>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Projectが存在しない場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();

        _projectRepository
            .Setup(x => x.GetByIdAsync(ticket.ProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var notification = new MemberAssignedNotification(
            [Guid.NewGuid(), Guid.NewGuid()],
            ticket.Id,
            ticket.Title.Value,
            Guid.NewGuid(),
            "テスト 太郎",
            ticket.ProjectId
        );
        var act = async () => await _handler.Handle(notification, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.PROJECT_NOT_FOUND");
        _notificationRepository.Verify(x => x.AddRangeAsync(
            It.IsAny<List<Notification>>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
