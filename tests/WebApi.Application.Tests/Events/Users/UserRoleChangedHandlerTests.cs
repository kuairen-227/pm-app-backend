using FluentAssertions;
using Moq;
using WebApi.Application.Common;
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
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserBuilder _userBuilder;

    public UserRoleChangedHandlerTests()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _userRepository = new Mock<IUserRepository>();
        _notificationFactory = new UserNotificationFactory(Clock);
        _userBuilder = new UserBuilder();

        _handler = new UserRoleChangedHandler(
            _notificationFactory,
            _notificationRepository.Object,
            _userRepository.Object,
            UnitOfWork.Object,
            UserContext.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var user = _userBuilder.Build();

        _userRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var notification = new UserRoleChangedNotification(user.Id, SystemRole.RoleType.User);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
