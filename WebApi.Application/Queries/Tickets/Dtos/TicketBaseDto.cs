namespace WebApi.Application.Queries.Tickets.Dtos;

public abstract class TicketBaseDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public Guid? AssigneeId { get; init; }
    public DateOnly? Deadline { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? CompletionCriteria { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UpdatedBy { get; init; }
    public DateTime UpdatedAt { get; init; }
}
