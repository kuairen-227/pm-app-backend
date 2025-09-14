namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketCommentDto
{
    public Guid AuthorId { get; init; }
    public string Content { get; init; } = string.Empty;
}
