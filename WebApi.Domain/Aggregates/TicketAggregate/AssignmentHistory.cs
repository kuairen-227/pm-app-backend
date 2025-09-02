using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class AssignmentHistory : ValueObject
{
    public enum AssignmentChangeType { Assigned, Changed, Unassigned }
    public AssignmentChangeType ChangeType { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public Guid? PreviousAssigneeId { get; private set; }
    public DateTime ChangedAt { get; private set; }

    private AssignmentHistory(
        AssignmentChangeType changeType,
        Guid? assigneeId,
        Guid? previousAssigneeId,
        DateTime changedAt)
    {
        ChangeType = changeType;
        AssigneeId = assigneeId;
        PreviousAssigneeId = previousAssigneeId;
        ChangedAt = changedAt;
    }

    public static AssignmentHistory Assigned(Guid assigneeId, DateTime assignedAt)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");

        return new AssignmentHistory(
            AssignmentChangeType.Assigned,
            assigneeId,
            null,
            assignedAt);
    }

    public static AssignmentHistory Changed(Guid assigneeId, Guid previousAssigneeId, DateTime changedAt)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");
        if (previousAssigneeId == Guid.Empty)
            throw new DomainException("PREVIOUS_ASSIGNEE_ID_REQUIRED", "Previous Assignee ID は必須です");

        return new AssignmentHistory(
            AssignmentChangeType.Changed,
            assigneeId,
            previousAssigneeId,
            changedAt);
    }

    public static AssignmentHistory Unassigned(Guid previousAssigneeId, DateTime unassignedAt)
    {
        if (previousAssigneeId == Guid.Empty)
            throw new DomainException("PREVIOUS_ASSIGNEE_ID_REQUIRED", "Previous Assignee ID は必須です");

        return new AssignmentHistory(
            AssignmentChangeType.Unassigned,
            null,
            previousAssigneeId,
            unassignedAt);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ChangeType;
        yield return AssigneeId;
        yield return PreviousAssigneeId;
        yield return ChangedAt;
    }
}
