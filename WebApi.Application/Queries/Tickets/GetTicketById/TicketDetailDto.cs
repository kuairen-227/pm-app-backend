using WebApi.Application.Queries.Tickets;
using WebApi.Application.Queries.Tickets.Dtos;

public class TicketDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public Guid? AssigneeId { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public string Status { get; set; } = null!;
    public string? CompletionCriteria { get; set; }
    public IReadOnlyList<TicketCommentDto> Comments { get; set; } = null!;
    public IReadOnlyList<AssignmentHistoryDto> AssignmentHistories { get; set; } = null!;

    public static TicketDetailDto From(
        TicketDto ticket, IEnumerable<TicketCommentDto> comments, IEnumerable<AssignmentHistoryDto> assignmentHistories)
    {
        return new TicketDetailDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            AssigneeId = ticket.AssigneeId,
            Deadline = ticket.Deadline,
            Status = ticket.Status,
            CompletionCriteria = ticket.CompletionCriteria,
            Comments = [.. comments],
            AssignmentHistories = [.. assignmentHistories],
        };
    }
}
