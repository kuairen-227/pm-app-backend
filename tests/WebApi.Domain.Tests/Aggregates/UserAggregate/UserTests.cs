using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class UserTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var user = new UserBuilder().Build();

        // Assert
        user.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Nameが空の場合(string? name)
    {
        // Arrange & Act
        Action act = () => new UserBuilder().WithName(name!).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("USER_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_AddNotification_1件()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var notification = new NotificationBuilder().Build();

        // Act
        user.AddNotification(notification);

        // Assert
        user.Notifications.Should().Contain(notification);
    }

    [Fact]
    public void 正常系_AddNotification_2件()
    {
        // Arrange
        var user = new UserBuilder().Build();
        var notification1 = new NotificationBuilder().Build();
        var notification2 = new NotificationBuilder().Build();

        // Act
        user.AddNotification(notification1);
        user.AddNotification(notification2);

        // Assert
        user.Notifications.Should().Contain(new[] { notification1, notification2 });
    }
}
