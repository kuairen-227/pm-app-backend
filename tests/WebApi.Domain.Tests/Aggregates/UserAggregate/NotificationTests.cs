using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class NotificationTests
{
    private readonly NotificationBuilder _notificationBuilder;

    public NotificationTests()
    {
        _notificationBuilder = new NotificationBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _notificationBuilder.Build();

        // Assertion
        result.Should().NotBeNull();
        result.IsRead.Should().BeFalse();
    }

    [Fact]
    public void 異常系_インスタンス生成_UserIdが空の場合()
    {
        // Arrange
        var builder = _notificationBuilder.WithUserId(Guid.Empty);

        // Act
        var act = () => builder.Build();

        // Then
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("NOTIFICATION_USER_ID_REQUIRED");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Messageが空の場合(string? message)
    {
        // Arrange
        var builder = _notificationBuilder.WithMessage(message!);

        // Act
        var act = () => builder.Build();

        // Then
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("NOTIFICATION_MESSAGE_REQUIRED");
    }

    [Fact]
    public void 正常系_MarkAsRead()
    {
        // Arrange
        var result = _notificationBuilder.Build();

        // Act
        result.MarkAsRead();

        // Assert
        result.IsRead.Should().BeTrue();
    }
}
