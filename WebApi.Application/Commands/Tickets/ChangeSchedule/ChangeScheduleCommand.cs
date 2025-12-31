using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.ChangeStatus;

[RequiresPermission(TicketPermissions.Update)]
public class ChangeScheduleCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public DateOnly? StartDate { get; }
    public DateOnly? EndDate { get; }
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; }

    public ChangeScheduleCommand(
        Guid projectId,
        Guid ticketId,
        DateOnly? startDate,
        DateOnly? endDate,
        IReadOnlyCollection<Guid> notificationRecipientIds
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        StartDate = startDate;
        EndDate = endDate;
        NotificationRecipientIds = notificationRecipientIds;
    }
}
