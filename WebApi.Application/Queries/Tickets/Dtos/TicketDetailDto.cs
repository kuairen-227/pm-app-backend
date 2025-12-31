namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketDetailDto : TicketBaseDto
{
    public IReadOnlyList<TicketCommentDto> Comments { get; set; } = Array.Empty<TicketCommentDto>();
    public IReadOnlyList<TicketHistoryDto> Histories { get; set; } = Array.Empty<TicketHistoryDto>();
}
