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
        var ticketTitle = TicketTitle.Create(title);

        // Assert
        ticketTitle.Value.Should().Be(title);
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
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("TICKET_TITLE_REQUIRED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var title1 = TicketTitle.Create("テストチケット");
        var title2 = TicketTitle.Create("テストチケット");

        // Assert
        title1.Should().Be(title2);
        title1.GetHashCode().Should().Be(title2.GetHashCode());
        title1.Equals(title2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var title1 = TicketTitle.Create("テストチケット");
        var title2 = TicketTitle.Create("テストチケット2");

        // Assert
        title1.Should().NotBe(title2);
        title1.GetHashCode().Should().NotBe(title2.GetHashCode());
        title1.Equals(title2).Should().BeFalse();
    }
}
