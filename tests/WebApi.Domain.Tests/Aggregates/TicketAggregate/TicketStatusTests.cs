using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketStatusTests
{
    [Theory]
    [InlineData(TicketStatus.StatusType.Todo)]
    [InlineData(TicketStatus.StatusType.InProgress)]
    [InlineData(TicketStatus.StatusType.Resolved)]
    [InlineData(TicketStatus.StatusType.Done)]
    public void 正常系_インスタンス生成(TicketStatus.StatusType statusType)
    {
        // Act
        var result = TicketStatus.Create(statusType);

        // Assert
        result.Value.Should().Be(statusType);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = TicketStatus.Create(TicketStatus.StatusType.Todo);
        var result2 = TicketStatus.Create(TicketStatus.StatusType.Todo);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var result1 = TicketStatus.Create(TicketStatus.StatusType.Todo);
        var result2 = TicketStatus.Create(TicketStatus.StatusType.InProgress);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
