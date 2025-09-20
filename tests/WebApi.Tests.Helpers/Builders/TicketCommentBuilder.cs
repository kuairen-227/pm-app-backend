using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class TicketCommentBuilder : BaseBuilder<TicketCommentBuilder, TicketComment>
{
    private Guid _ticketId = Guid.NewGuid();
    private Guid _authorId = Guid.NewGuid();
    private string _content = "デフォルトのコメント内容";

    public TicketCommentBuilder WithTicketId(Guid ticketId)
    {
        _ticketId = ticketId;
        return this;
    }

    public TicketCommentBuilder WithAuthorId(Guid authorId)
    {
        _authorId = authorId;
        return this;
    }

    public TicketCommentBuilder WithContent(string content)
    {
        _content = content;
        return this;
    }

    public override TicketComment Build()
    {
        return TicketComment.Create(
            _ticketId,
            _authorId,
            _content,
            _createdBy,
            _clock
        );
    }
}
