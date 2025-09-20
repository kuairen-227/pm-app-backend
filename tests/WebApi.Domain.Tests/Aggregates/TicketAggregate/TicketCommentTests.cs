using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketCommentTests
{
    private readonly TicketCommentBuilder _commentBuilder;

    public TicketCommentTests()
    {
        _commentBuilder = new TicketCommentBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _commentBuilder.Build();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void 異常系_インスタンス生成_TicketIdが空の場合()
    {
        // Arrange & Act
        Action act = () => _commentBuilder.WithTicketId(Guid.Empty).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("TICKET_ID_REQUIRED");
    }

    [Fact]
    public void 異常系_インスタンス生成_AuthorIdが空の場合()
    {
        // Arrange & Act
        Action act = () => _commentBuilder.WithAuthorId(Guid.Empty).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("AUTHOR_ID_REQUIRED");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_Contentが空の場合(string? content)
    {
        // Arrange & Act
        Action act = () => _commentBuilder.WithContent(content!).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("CONTENT_REQUIRED");
    }

    [Fact]
    public void 正常系_UpdateContent()
    {
        // Arrange
        var result = _commentBuilder.Build();

        // Act
        result.UpdateContent("編集コメント");

        // Assert
        result.Content.Should().Be("編集コメント");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_UpdateContent_Contentが空の場合(string? content)
    {
        // Arrange
        var ticketComment = _commentBuilder.Build();

        // Act
        Action act = () => ticketComment.UpdateContent(content!);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("CONTENT_REQUIRED");
    }
}
