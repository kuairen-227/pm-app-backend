namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketCompletionCriterionDto
{
    public Guid Id { get; init; }
    public string Criterion { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UpdatedBy { get; init; }
    public DateTime UpdatedAt { get; init; }
}
