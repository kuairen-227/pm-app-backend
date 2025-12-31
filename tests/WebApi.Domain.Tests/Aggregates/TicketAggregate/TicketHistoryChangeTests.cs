using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketHistoryChangeTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData("Old Value", null)]
    [InlineData(null, "New Value")]
    [InlineData("Old Value", "New Value")]
    public void 正常系_インスタンス生成(string? before, string? after)
    {
        // Arrange & Act
        var result = TicketHistoryChange.Create(
            TicketField.Title,
            TicketHistoryChangeValue.From(before),
            TicketHistoryChangeValue.From(after)
        );

        // Assert
        result.Field.Should().Be(TicketField.Title);
        result.Before?.Value.Should().Be(before);
        result.After?.Value.Should().Be(after);
    }

    [Fact]
    public void 異常系_インスタンス生成()
    {
        // Arrange
        var before = "Same Value";
        var after = "Same Value";

        // Act
        var act = () => TicketHistoryChange.Create(
            TicketField.Title,
            TicketHistoryChangeValue.From(before),
            TicketHistoryChangeValue.From(after)
        );

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NO_CHANGE_IN_HISTORY");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange
        var field = TicketField.Description;
        var before = TicketHistoryChangeValue.From("Old Description");
        var after = TicketHistoryChangeValue.From("New Description");

        // Act
        var result1 = TicketHistoryChange.Create(field, before, after);
        var result2 = TicketHistoryChange.Create(field, before, after);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var field = TicketField.Description;
        var before1 = TicketHistoryChangeValue.From("Old Description 1");
        var after1 = TicketHistoryChangeValue.From("New Description 1");
        var before2 = TicketHistoryChangeValue.From("Old Description 2");
        var after2 = TicketHistoryChangeValue.From("New Description 2");

        // Act
        var result1 = TicketHistoryChange.Create(field, before1, after1);
        var result2 = TicketHistoryChange.Create(field, before2, after2);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
