using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class AssignmentHistory : Entity
{
    public Guid TicketId { get; private set; }
    public Guid AssigneeId { get; private set; }
    public DateTime AssignedAt { get; private set; }

    public AssignmentHistory(Guid ticketId, Guid assigneeId)
    {
        TicketId = ticketId;
        AssigneeId = assigneeId;
        AssignedAt = DateTime.UtcNow;
    }
}
