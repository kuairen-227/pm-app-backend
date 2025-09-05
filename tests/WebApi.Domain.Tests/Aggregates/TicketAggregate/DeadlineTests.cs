using FluentAssertions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class DeadlineTests
{
    private readonly IDateTimeProvider _clock;

    public DeadlineTests()
    {
        _clock = new FakeDateTimeProvider();
    }

    [Fact]
    public void 正常系_インスタンス生成_未来日付()
    {
        // Arrange
        var tomorrow = _clock.Now.Date.AddDays(1);

        // Act
        var result = Deadline.Create(tomorrow, _clock);

        // Assert
        result.Value.Should().Be(tomorrow);
    }

    [Fact]
    public void 異常系_インスタンス生成_現在時刻()
    {
        // Arrange
        var now = _clock.Now;

        // Act
        Action act = () => Deadline.Create(now, _clock);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("DEADLINE_PAST_NOT_ALLOWED");
    }

    [Fact]
    public void 異常系_インスタンス生成_過去日付()
    {
        // Arrange
        var yesterday = _clock.Now.Date.AddDays(-1);

        // Act
        Action act = () => Deadline.Create(yesterday, _clock);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("DEADLINE_PAST_NOT_ALLOWED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = Deadline.Create(_clock.Now.Date.AddDays(1), _clock);
        var result2 = Deadline.Create(_clock.Now.Date.AddDays(1), _clock);

        // Then
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Assert & Act
        var result1 = Deadline.Create(_clock.Now.Date.AddDays(1), _clock);
        var result2 = Deadline.Create(_clock.Now.Date.AddDays(2), _clock);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
