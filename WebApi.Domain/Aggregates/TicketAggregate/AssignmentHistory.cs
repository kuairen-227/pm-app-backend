using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class AssignmentHistory : ValueObject
{
    public Guid Id { get; private set; }  // EF Core 用
    public enum AssignmentChangeType { Assigned, Changed, Unassigned }
    public AssignmentChangeType ChangeType { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public Guid? PreviousAssigneeId { get; private set; }
    public DateTime ChangedAt { get; private set; }
    public AuditInfo AuditInfo { get; private set; } = null!;  // DB 用の監査情報

    private AssignmentHistory() { } // EF Core 用

    private AssignmentHistory(
        AssignmentChangeType changeType,
        Guid? assigneeId,
        Guid? previousAssigneeId,
        Guid createdBy,
        IDateTimeProvider clock)
    {
        ChangeType = changeType;
        AssigneeId = assigneeId;
        PreviousAssigneeId = previousAssigneeId;
        ChangedAt = clock.Now;
        AuditInfo = new AuditInfo(createdBy, clock);
    }

    public static AssignmentHistory Assigned(
        Guid assigneeId, Guid assignedBy, IDateTimeProvider clock)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");

        return new AssignmentHistory(
            AssignmentChangeType.Assigned,
            assigneeId,
            null,
            assignedBy,
            clock);
    }

    public static AssignmentHistory Changed(
        Guid assigneeId, Guid previousAssigneeId, Guid changedBy, IDateTimeProvider clock)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");
        if (previousAssigneeId == Guid.Empty)
            throw new DomainException("PREVIOUS_ASSIGNEE_ID_REQUIRED", "Previous Assignee ID は必須です");

        return new AssignmentHistory(
            AssignmentChangeType.Changed,
            assigneeId,
            previousAssigneeId,
            changedBy,
            clock);
    }

    public static AssignmentHistory Unassigned(
        Guid previousAssigneeId, Guid changedBy, IDateTimeProvider clock)
    {
        if (previousAssigneeId == Guid.Empty)
            throw new DomainException("PREVIOUS_ASSIGNEE_ID_REQUIRED", "Previous Assignee ID は必須です");

        return new AssignmentHistory(
            AssignmentChangeType.Unassigned,
            null,
            previousAssigneeId,
            changedBy,
            clock);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ChangeType;
        yield return AssigneeId;
        yield return PreviousAssigneeId;
        yield return ChangedAt;
    }
}
