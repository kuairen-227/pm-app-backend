using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate.Events;

public sealed class TicketMemberAssignedEvent : DomainEvent
{
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; }
    public Guid TicketId { get; }
    public TicketTitle TicketTitle { get; }
    public Guid AssigneeId { get; }
    public string AssigneeName { get; }
    public Guid ProjectId { get; }

    public TicketMemberAssignedEvent(
        IReadOnlyCollection<Guid> notificationRecipientIds,
        Guid ticketId,
        TicketTitle ticketTitle,
        Guid assigneeId,
        string assigneeName,
        Guid projectId,
        IDateTimeProvider clock
    ) : base(clock)
    {
        NotificationRecipientIds = notificationRecipientIds;
        TicketId = ticketId;
        TicketTitle = ticketTitle;
        AssigneeId = assigneeId;
        AssigneeName = assigneeName;
        ProjectId = projectId;
    }
}
