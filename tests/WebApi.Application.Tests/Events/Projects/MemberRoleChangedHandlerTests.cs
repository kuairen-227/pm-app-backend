using FluentAssertions;
using Moq;
using WebApi.Application.Common;
using WebApi.Application.Events.Projects.MemberRoleChanged;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Events.Projects;

public class MemberRoleChangedHandlerTests : BaseEventHandlerTest
{
    private MemberRoleChangedHandler _handler;
    private readonly ProjectNotificationFactory _notificationFactory;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly ProjectBuilder _projectBuilder;
    private readonly UserBuilder _userBuilder;

    public MemberRoleChangedHandlerTests()
    {
        _notificationFactory = new ProjectNotificationFactory(Clock);
        _notificationRepository = new Mock<INotificationRepository>();
        _projectBuilder = new ProjectBuilder();
        _userBuilder = new UserBuilder();

        _handler = new MemberRoleChangedHandler(
            _notificationFactory,
            _notificationRepository.Object,
            UnitOfWork.Object,
            UserContext.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var user = _userBuilder.Build();

        Notification? capturedNotification = null;
        _notificationRepository
            .Setup(x => x.AddAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Callback<Notification, CancellationToken>((n, _) => capturedNotification = n)
            .Returns(Task.CompletedTask);

        // Act
        var notification = new MemberRoleChangedNotification(project.Id, user.Id, ProjectRole.RoleType.Member);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        capturedNotification.Should().NotBeNull();
        capturedNotification.RecipientId.Should().Be(user.Id);
        capturedNotification.Category.Value.Should().Be(NotificationCategory.Category.ProjectMemberRoleChanged);
        capturedNotification.SubjectId.Should().Be(project.Id);
        capturedNotification.Message.Should().Be($"メンバー権限が {notification.Role} に変更されました。");
        capturedNotification.IsRead.Should().Be(false);

        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
