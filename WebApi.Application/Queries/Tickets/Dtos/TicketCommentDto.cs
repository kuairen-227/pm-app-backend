namespace WebApi.Application.Queries.Tickets;

public sealed class TicketCommentDto
{
    public Guid AuthorId { get; init; }
    public string Content { get; init; } = string.Empty;
}
