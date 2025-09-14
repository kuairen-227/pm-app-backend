namespace WebApi.Application.Queries.Tickets;

public sealed class AssignmentHistoryDto
{
    public string ChangeType { get; init; } = string.Empty;
    public Guid? AssigneeId { get; init; }
    public Guid? PreviousAssigneeId { get; init; }
    public DateTimeOffset ChangedAt { get; init; }
}
