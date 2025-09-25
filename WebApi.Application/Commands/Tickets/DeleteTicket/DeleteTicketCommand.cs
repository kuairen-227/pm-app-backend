using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.DeleteTicket;

[RequiresPermission(TicketPermissions.Delete)]
public class DeleteTicketCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }

    public DeleteTicketCommand(Guid projectId, Guid ticketId)
    {
        ProjectId = projectId;
        TicketId = ticketId;
    }
}
