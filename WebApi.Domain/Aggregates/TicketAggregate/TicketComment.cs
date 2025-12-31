using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketComment : Entity
{
    public Guid AuthorId { get; private set; }
    public string Content { get; private set; } = null!;

    private TicketComment() { } // EF Core 用

    public TicketComment(Guid authorId, string content, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (authorId == Guid.Empty)
            throw new DomainException("AUTHOR_ID_REQUIRED", "Author ID は必須です");
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("CONTENT_REQUIRED", "Content は必須です");

        AuthorId = authorId;
        Content = content;
    }

    public void UpdateContent(string content, Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("CONTENT_REQUIRED", "Content は必須です");

        Content = content;
        UpdateAuditInfo(updatedBy);
    }
}
