using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.UnassignMember;

[RequiresPermission(TicketPermissions.Unassign)]
public class UnassignMemberCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }

    public UnassignMemberCommand(Guid projectId, Guid ticketId)
    {
        ProjectId = projectId;
        TicketId = ticketId;
    }
}
