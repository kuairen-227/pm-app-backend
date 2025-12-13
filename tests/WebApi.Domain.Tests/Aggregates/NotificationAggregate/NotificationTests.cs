using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.NotificationAggregate;

public class NotificationTests : BaseDomainTest
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
        var builder = _notificationBuilder.WithRecipientId(Guid.Empty);

        // Act
        var act = () => builder.Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NOTIFICATION_RECIPIENT_ID_REQUIRED");
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

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NOTIFICATION_MESSAGE_REQUIRED");
    }

    [Fact]
    public void 正常系_MarkAsRead()
    {
        // Arrange
        var result = _notificationBuilder.WithRecipientId(UserContext.Id).Build();

        // Act
        result.MarkAsRead(UserContext.Id);

        // Assert
        result.IsRead.Should().BeTrue();
    }

    [Fact]
    public void 異常系_MarkAsRead_通知の対象ユーザー出ない場合()
    {
        // Arrange
        var notification = _notificationBuilder.WithRecipientId(Guid.NewGuid()).Build();

        // Act
        var act = () => notification.MarkAsRead(UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NOT_NOTIFICATION_RECIPIENT");
    }
}
