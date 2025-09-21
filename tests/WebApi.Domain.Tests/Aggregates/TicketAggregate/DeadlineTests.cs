using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class DeadlineTests : BaseDomainTest
{
    [Fact]
    public void 正常系_インスタンス生成_未来日付()
    {
        // Arrange
        var tomorrow = Clock.Today.AddDays(1);

        // Act
        var result = Deadline.Create(tomorrow, Clock);

        // Assert
        result.Value.Should().Be(tomorrow);
    }

    [Fact]
    public void 異常系_インスタンス生成_現在日付()
    {
        // Arrange
        var today = Clock.Today;

        // Act
        var act = () => Deadline.Create(today, Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DEADLINE_PAST_NOT_ALLOWED");
    }

    [Fact]
    public void 異常系_インスタンス生成_過去日付()
    {
        // Arrange
        var yesterday = Clock.Today.AddDays(-1);

        // Act
        var act = () => Deadline.Create(yesterday, Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DEADLINE_PAST_NOT_ALLOWED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = Deadline.Create(Clock.Today.AddDays(1), Clock);
        var result2 = Deadline.Create(Clock.Today.AddDays(1), Clock);

        // Then
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Assert & Act
        var result1 = Deadline.Create(Clock.Today.AddDays(1), Clock);
        var result2 = Deadline.Create(Clock.Today.AddDays(2), Clock);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
