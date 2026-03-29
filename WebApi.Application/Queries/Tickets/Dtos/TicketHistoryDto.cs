namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketHistoryDto
{
    public Guid Id { get; init; }
    public Guid ActorId { get; init; }
    public DateTime OccurredAt { get; init; }
    public string Action { get; init; } = string.Empty;
    public IReadOnlyList<TicketHistoryChangeDto> Changes { get; init; } = Array.Empty<TicketHistoryChangeDto>();
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UpdatedBy { get; init; }
    public DateTime UpdatedAt { get; init; }
}
