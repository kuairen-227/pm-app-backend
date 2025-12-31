namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketHistoryChangeDto
{
    public string Field { get; init; } = string.Empty;
    public string? Before { get; init; }
    public string? After { get; init; }
}
