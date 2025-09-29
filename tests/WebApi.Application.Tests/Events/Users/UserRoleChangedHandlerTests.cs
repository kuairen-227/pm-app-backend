using FluentAssertions;
using Moq;
using WebApi.Application.Events.Users.UserRoleChanged;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Events.Users;

public class UserRoleChangedHandlerTests : BaseEventHandlerTest
{
    private UserRoleChangedHandler _handler;
    private readonly UserNotificationFactory _notificationFactory;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly UserBuilder _userBuilder;

    public UserRoleChangedHandlerTests()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationFactory = new UserNotificationFactory(Clock);
        _userBuilder = new UserBuilder();

        _handler = new UserRoleChangedHandler(
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
        var user = _userBuilder.Build();

        Notification? capturedNotification = null;
        _notificationRepository
            .Setup(x => x.AddAsync(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
            .Callback<Notification, CancellationToken>((n, _) => capturedNotification = n)
            .Returns(Task.CompletedTask);

        // Act
        var notification = new UserRoleChangedNotification(user.Id, SystemRole.RoleType.User);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        capturedNotification.Should().NotBeNull();
        capturedNotification.RecipientId.Should().Be(user.Id);
        capturedNotification.Category.Value.Should().Be(NotificationCategory.Category.UserRoleChanged);
        capturedNotification.SubjectId.Should().Be(user.Id);
        capturedNotification.Message.Should().Be($"ユーザー権限が {notification.Role} に変更されました。");
        capturedNotification.IsRead.Should().Be(false);

        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
