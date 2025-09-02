using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class DeadlineTests
{
    [Fact]
    public void 正常系_インスタンス生成_今日日付()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;

        // Act
        var deadline = Deadline.Create(today);

        // Assert
        deadline.Value.Should().Be(today);
    }

    [Fact]
    public void 正常系_インスタンス生成_未来日付()
    {
        // Arrange
        var tomorrow = DateTime.UtcNow.Date.AddDays(1);

        // Act
        var deadline = Deadline.Create(tomorrow);

        // Assert
        deadline.Value.Should().Be(tomorrow);
    }

    [Fact]
    public void 異常系_インスタンス生成_過去日付()
    {
        // Arrange
        var yesterday = DateTime.UtcNow.Date.AddDays(-1);

        // Act
        Action act = () => Deadline.Create(yesterday);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("DEADLINE_PAST_NOT_ALLOWED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var deadline1 = Deadline.Create(DateTime.UtcNow.Date.AddDays(1));
        var deadline2 = Deadline.Create(DateTime.UtcNow.Date.AddDays(1));

        // Then
        deadline1.Should().Be(deadline2);
        deadline1.GetHashCode().Should().Be(deadline2.GetHashCode());
        deadline1.Equals(deadline2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Assert & Act
        var deadline1 = Deadline.Create(DateTime.UtcNow.Date.AddDays(1));
        var deadline2 = Deadline.Create(DateTime.UtcNow.Date.AddDays(2));

        // Assert
        deadline1.Should().NotBe(deadline2);
        deadline1.GetHashCode().Should().NotBe(deadline2.GetHashCode());
        deadline1.Equals(deadline2).Should().BeFalse();
    }
}
