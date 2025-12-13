using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketDescriptionTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange
        var description = "テストチケットの説明";

        // Act
        var result = TicketDescription.Create(description);

        // Assert
        result.Value.Should().Be(description);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = TicketDescription.Create("テストチケットの説明");
        var result2 = TicketDescription.Create("テストチケットの説明");

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var result1 = TicketDescription.Create("テストチケットの説明");
        var result2 = TicketDescription.Create("テストチケットの説明2");

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
