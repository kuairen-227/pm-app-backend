using FluentAssertions;
using Moq;
using WebApi.Application.Common;
using WebApi.Application.Events.Users.UserRegistered;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Events.Users;

public class UserRegisteredHandlerTests : BaseEventHandlerTest
{
    private UserRegisteredHandler _handler;
    private readonly UserNotificationFactory _notificationFactory;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly UserBuilder _userBuilder;

    public UserRegisteredHandlerTests()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _userRepository = new Mock<IUserRepository>();
        _notificationFactory = new UserNotificationFactory(Clock);
        _userBuilder = new UserBuilder();

        _handler = new UserRegisteredHandler(
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
        var notification = new UserRegisteredNotification(user.Id, user.Name);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_Userが存在しない場合()
    {
        // Arrange
        _userRepository.Setup(x => x.GetByIdAsync(Guid.NewGuid(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var notification = new UserRegisteredNotification(Guid.NewGuid(), "存在しないユーザー");
        var act = async () => await _handler.Handle(notification, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("USER_NOT_FOUND");
        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
