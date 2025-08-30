using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class TicketComment : Entity
{
    public Guid TicketId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Content { get; private set; } = null!;

    private TicketComment() { } // EF Core 用

    private TicketComment(Guid ticketId, Guid authorId, string content)
    {
        if (ticketId == Guid.Empty)
            throw new DomainException("TICKET_ID_REQUIRED", "Ticket ID は必須です");
        if (authorId == Guid.Empty)
            throw new DomainException("AUTHOR_ID_REQUIRED", "Author ID は必須です");
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("CONTENT_REQUIRED", "Content は必須です");

        TicketId = ticketId;
        AuthorId = authorId;
        Content = content;
    }

    public static TicketComment Create(Guid ticketId, Guid authorId, string content)
    {
        return new TicketComment(ticketId, authorId, content);
    }

    public void UpdateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("CONTENT_REQUIRED", "Content は必須です");

        Content = content;
    }
}
