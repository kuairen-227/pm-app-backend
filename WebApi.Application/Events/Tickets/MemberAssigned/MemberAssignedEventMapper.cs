using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate.Events;

namespace WebApi.Application.Events.Tickets.MemberAssigned;

public sealed class MemberAssignedEventMapper : IDomainEventMapper<TicketMemberAssignedEvent>
{
    public INotification? Map(TicketMemberAssignedEvent e) =>
        new MemberAssignedNotification(
            e.NotificationRecipientIds,
            e.TicketId,
            e.TicketTitle.Value,
            e.AssigneeId,
            e.AssigneeName,
            e.ProjectId
        );
}
