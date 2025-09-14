namespace WebApi.Application.Queries.Tickets;

public class AssignmentHistoryDto
{
    public string ChangeType { get; set; } = null!;
    public Guid? AssigneeId { get; set; }
    public Guid? PreviousAssigneeId { get; set; }
    public DateTimeOffset ChangedAt { get; set; }
}
