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
        var status = TicketStatus.Create(statusType);

        // Assert
        status.Value.Should().Be(statusType);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var status1 = TicketStatus.Create(TicketStatus.StatusType.Todo);
        var status2 = TicketStatus.Create(TicketStatus.StatusType.Todo);

        // Assert
        status1.Should().Be(status2);
        status1.GetHashCode().Should().Be(status2.GetHashCode());
        status1.Equals(status2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var status1 = TicketStatus.Create(TicketStatus.StatusType.Todo);
        var status2 = TicketStatus.Create(TicketStatus.StatusType.InProgress);

        // Assert
        status1.Should().NotBe(status2);
        status1.GetHashCode().Should().NotBe(status2.GetHashCode());
        status1.Equals(status2).Should().BeFalse();
    }
}
