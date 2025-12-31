namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketHistoryDto
{
    public Guid ActorId { get; init; }
    public DateTime OccurredAt { get; init; }
    public string Action { get; init; } = string.Empty;
    public IReadOnlyList<TicketHistoryChangeDto> Changes { get; init; } = Array.Empty<TicketHistoryChangeDto>();
}
