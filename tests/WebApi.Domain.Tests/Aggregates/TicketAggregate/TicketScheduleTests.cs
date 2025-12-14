using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketScheduleTests : BaseDomainTest
{
    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "2025-12-31")]
    [InlineData("2025-01-01", null)]
    [InlineData("2025-01-01", "2025-12-31")]
    [InlineData("2025-01-01", "2025-01-01")]
    public void 正常系_インスタンス生成(string? startDateValue, string? endDateValue)
    {
        // Arrange
        DateOnly? startDate = startDateValue is null ? null : DateOnly.Parse(startDateValue);
        DateOnly? endDate = endDateValue is null ? null : DateOnly.Parse(endDateValue);

        // Act
        var result = TicketSchedule.Create(startDate, endDate);

        // Assert
        result.Should().NotBeNull();
        result.StartDate.Should().Be(startDate);
        result.EndDate.Should().Be(endDate);
    }

    [Fact]
    public void 異常系_インスタンス生成_終了日が開始日より前()
    {
        // Arrange
        var startDate = DateOnly.Parse("2025-01-01");
        var endDate = DateOnly.Parse("2024-12-31");

        // Act
        var act = () => TicketSchedule.Create(startDate, endDate);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.TICKET_SCHEDULE_INVALID");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var startDate = DateOnly.Parse("2025-01-01");
        var endDate = DateOnly.Parse("2025-12-31");
        var result1 = TicketSchedule.Create(startDate, endDate);
        var result2 = TicketSchedule.Create(startDate, endDate);

        // Then
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Assert & Act
        var startDate1 = DateOnly.Parse("2025-01-01");
        var endDate1 = DateOnly.Parse("2025-12-31");
        var startDate2 = DateOnly.Parse("2025-02-01");
        var endDate2 = DateOnly.Parse("2025-11-30");
        var result1 = TicketSchedule.Create(startDate1, endDate1);
        var result2 = TicketSchedule.Create(startDate2, endDate2);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
