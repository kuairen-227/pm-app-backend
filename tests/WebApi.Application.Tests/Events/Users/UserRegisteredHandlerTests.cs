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
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationFactory = new UserNotificationFactory(Clock);
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

        // Act
        var notification = new UserRegisteredNotification(user.Id, user.Name);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
