using MediatR;

namespace WebApi.Application.Events.Tickets.MemberAssigned;

public sealed class MemberAssignedNotification : INotification
{
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; }
    public Guid TicketId { get; }
    public string TicketTitle { get; }
    public Guid AssigneeId { get; }
    public string AssigneeName { get; }
    public Guid ProjectId { get; }

    public MemberAssignedNotification(
        IReadOnlyCollection<Guid> notificationRecipientIds,
        Guid ticketId,
        string ticketTitle,
        Guid assigneeId,
        string assigneeName,
        Guid projectId
    )
    {
        NotificationRecipientIds = notificationRecipientIds;
        TicketId = ticketId;
        TicketTitle = ticketTitle;
        AssigneeId = assigneeId;
        AssigneeName = assigneeName;
        ProjectId = projectId;
    }
}
