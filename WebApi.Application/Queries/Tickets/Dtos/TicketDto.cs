namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public Guid? AssigneeId { get; init; }
    public DateTimeOffset? Deadline { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? CompletionCriteria { get; init; }
}
