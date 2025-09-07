using FluentAssertions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class DeadlineTests : BaseTest
{
    [Fact]
    public void 正常系_インスタンス生成_未来日付()
    {
        // Arrange
        var tomorrow = Clock.Now.Date.AddDays(1);

        // Act
        var result = Deadline.Create(tomorrow, Clock);

        // Assert
        result.Value.Should().Be(tomorrow);
    }

    [Fact]
    public void 異常系_インスタンス生成_現在時刻()
    {
        // Arrange
        var now = Clock.Now;

        // Act
        Action act = () => Deadline.Create(now, Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DEADLINE_PAST_NOT_ALLOWED");
    }

    [Fact]
    public void 異常系_インスタンス生成_過去日付()
    {
        // Arrange
        var yesterday = Clock.Now.Date.AddDays(-1);

        // Act
        Action act = () => Deadline.Create(yesterday, Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DEADLINE_PAST_NOT_ALLOWED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = Deadline.Create(Clock.Now.Date.AddDays(1), Clock);
        var result2 = Deadline.Create(Clock.Now.Date.AddDays(1), Clock);

        // Then
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Assert & Act
        var result1 = Deadline.Create(Clock.Now.Date.AddDays(1), Clock);
        var result2 = Deadline.Create(Clock.Now.Date.AddDays(2), Clock);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
