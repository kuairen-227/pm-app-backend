using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.ChangeStatus;

[RequiresPermission(TicketPermissions.Update)]
public class ChangeStatusCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public TicketStatus.StatusType Status { get; }

    public ChangeStatusCommand(Guid projectId, Guid ticketId, TicketStatus.StatusType status)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Status = status;
    }
}
