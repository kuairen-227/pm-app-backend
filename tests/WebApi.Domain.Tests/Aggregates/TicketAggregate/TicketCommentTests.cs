using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketCommentTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var ticketComment = new TicketCommentBuilder().Build();

        // Assert
        ticketComment.Should().NotBeNull();
    }

    [Fact]
    public void 異常系_インスタンス生成_TicketIdが空の場合()
    {
        // Arrange & Act
        Action act = () => new TicketCommentBuilder().WithTicketId(Guid.Empty).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("TICKET_ID_REQUIRED");
    }

    [Fact]
    public void 異常系_インスタンス生成_AuthorIdが空の場合()
    {
        // Arrange & Act
        Action act = () => new TicketCommentBuilder().WithAuthorId(Guid.Empty).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("AUTHOR_ID_REQUIRED");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Contentが空の場合(string? content)
    {
        // Arrange & Act
        Action act = () => new TicketCommentBuilder().WithContent(content!).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("CONTENT_REQUIRED");
    }

    [Fact]
    public void 正常系_UpdateContent()
    {
        // Arrange
        var ticketComment = new TicketCommentBuilder().Build();

        // Act
        ticketComment.UpdateContent("編集コメント");

        // Assert
        ticketComment.Content.Should().Be("編集コメント");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_UpdateContent_Contentが空の場合(string? content)
    {
        // Arrange
        var ticketComment = new TicketCommentBuilder().Build();

        // Act
        Action act = () => ticketComment.UpdateContent(content!);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("CONTENT_REQUIRED");
    }
}
