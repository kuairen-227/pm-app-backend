using WebApi.Application.Queries.Tickets;

public sealed class TicketDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public Guid? AssigneeId { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public string Status { get; set; } = null!;
    public string? CompletionCriteria { get; set; }
    public IReadOnlyList<TicketCommentDto> Comments { get; set; } = null!;
    public IReadOnlyList<AssignmentHistoryDto> AssignmentHistories { get; set; } = null!;
}
