namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketCompletionCriterionDto
{
    public string Criterion { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
}
