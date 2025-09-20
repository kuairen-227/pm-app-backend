using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class UserTests
{
    private readonly UserBuilder _userBuilder;

    public UserTests()
    {
        _userBuilder = new UserBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _userBuilder.Build();

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Nameが空の場合(string? name)
    {
        // Arrange & Act
        Action act = () => _userBuilder.WithName(name!).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("USER_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_AddNotification_1件()
    {
        // Arrange
        var notification = new NotificationBuilder().Build();

        // Act
        var result = _userBuilder.Build();
        result.AddNotification(notification);

        // Assert
        result.Notifications.Should().Contain(notification);
    }

    [Fact]
    public void 正常系_AddNotification_2件()
    {
        // Arrange
        var notification1 = new NotificationBuilder().Build();
        var notification2 = new NotificationBuilder().Build();

        // Act
        var result = _userBuilder.Build();
        result.AddNotification(notification1);
        result.AddNotification(notification2);

        // Assert
        result.Notifications.Should().Contain([notification1, notification2]);
    }
}
