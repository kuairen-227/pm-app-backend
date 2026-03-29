namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketCommentDto
{
    public Guid Id { get; init; }
    public Guid AuthorId { get; init; }
    public string Content { get; init; } = string.Empty;
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UpdatedBy { get; init; }
    public DateTime UpdatedAt { get; init; }
}
