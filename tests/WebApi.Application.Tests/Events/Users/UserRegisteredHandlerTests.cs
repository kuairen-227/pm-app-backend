using FluentAssertions;
using Moq;
using WebApi.Application.Events.Users.UserRegistered;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Events.Users;

public class UserRegisteredHandlerTests : BaseEventHandlerTest
{
    private UserRegisteredHandler _handler;
    private readonly UserNotificationFactory _notificationFactory;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly UserBuilder _userBuilder;

    public UserRegisteredHandlerTests()
    {
        _notificationFactory = new UserNotificationFactory(Clock);
        _notificationRepository = new Mock<INotificationRepository>();
        _userBuilder = new UserBuilder();

        _handler = new UserRegisteredHandler(
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
        var notification = new UserRegisteredNotification(user.Id, user.Name);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        capturedNotification.Should().NotBeNull();
        capturedNotification.RecipientId.Should().Be(user.Id);
        capturedNotification.Category.Value.Should().Be(NotificationCategory.Category.UserRegistered);
        capturedNotification.SubjectId.Should().Be(user.Id);
        capturedNotification.Message.Should().Be($"{user.Name} が登録されました。");
        capturedNotification.IsRead.Should().Be(false);

        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
