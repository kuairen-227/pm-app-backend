using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class NotificationTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var notification = new NotificationBuilder().Build();

        // Assertion
        notification.Should().NotBeNull();
        notification.IsRead.Should().BeFalse();
    }

    [Fact]
    public void 異常系_インスタンス生成_UserIdが空の場合()
    {
        // Arrange
        var builder = new NotificationBuilder().WithUserId(Guid.Empty);

        // Act
        Action act = () => builder.Build();

        // Then
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("NOTIFICATION_USER_ID_REQUIRED");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Messageが空の場合(string? message)
    {
        // Arrange
        var builder = new NotificationBuilder().WithMessage(message!);

        // Act
        Action act = () => builder.Build();

        // Then
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("NOTIFICATION_MESSAGE_REQUIRED");
    }

    [Fact]
    public void 正常系_MarkAsRead()
    {
        // Arrange
        var notification = new NotificationBuilder().Build();

        // Act
        notification.MarkAsRead();

        // Assert
        notification.IsRead.Should().BeTrue();
    }
}
