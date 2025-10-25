using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.ChangeStatus;

[RequiresPermission(TicketPermissions.Update)]
public class ChangeStatusCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public TicketStatus.StatusType Status { get; }
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; }

    public ChangeStatusCommand(
        Guid projectId,
        Guid ticketId,
        TicketStatus.StatusType status,
        IReadOnlyCollection<Guid> notificationRecipientIds
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Status = status;
        NotificationRecipientIds = notificationRecipientIds;
    }
}
