namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketDetailDto : TicketBaseDto
{
    public IReadOnlyList<TicketCommentDto> Comments { get; set; } = Array.Empty<TicketCommentDto>();
    public IReadOnlyList<AssignmentHistoryDto> AssignmentHistories { get; set; } = Array.Empty<AssignmentHistoryDto>();
}
