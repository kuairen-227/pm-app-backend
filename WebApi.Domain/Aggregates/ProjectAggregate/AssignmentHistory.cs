using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class AssignmentHistory : ValueObject
{
    public Guid AssigneeId { get; private set; }
    public DateTime AssignedAt { get; private set; }

    private AssignmentHistory(Guid assigneeId, DateTime assignedAt)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");

        AssigneeId = assigneeId;
        AssignedAt = assignedAt;
    }

    public static AssignmentHistory Create(Guid assigneeId, DateTime assignedAt)
    {
        return new AssignmentHistory(assigneeId, assignedAt);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return AssigneeId;
        yield return AssignedAt;
    }
}
