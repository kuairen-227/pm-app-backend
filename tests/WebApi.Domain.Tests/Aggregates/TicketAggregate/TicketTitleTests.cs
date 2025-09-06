using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketTitleTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange
        var title = "テストチケット";

        // Act
        var result = TicketTitle.Create(title);

        // Assert
        result.Value.Should().Be(title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Titleが空の場合(string? title)
    {
        // Act
        Action act = () => TicketTitle.Create(title!);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("TICKET_TITLE_REQUIRED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = TicketTitle.Create("テストチケット");
        var result2 = TicketTitle.Create("テストチケット");

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var result1 = TicketTitle.Create("テストチケット");
        var result2 = TicketTitle.Create("テストチケット2");

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
