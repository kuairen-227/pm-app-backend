namespace WebApi.Application.Queries.Tickets.Dtos;

public sealed class TicketDetailDto : TicketBaseDto
{
    public IReadOnlyList<TicketCompletionCriterionDto> CompletionCriteria { get; set; }
        = Array.Empty<TicketCompletionCriterionDto>();
    public IReadOnlyList<TicketCommentDto> Comments { get; set; }
        = Array.Empty<TicketCommentDto>();
    public IReadOnlyList<TicketHistoryDto> Histories { get; set; }
        = Array.Empty<TicketHistoryDto>();
}
