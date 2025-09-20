using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Commands.Tickets.AssignMember;

[RequiresPermission(TicketPermissions.Assign)]
public class AssignMemberCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Guid UserId { get; }

    public AssignMemberCommand(Guid projectId, Guid ticketId, Guid userId)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        UserId = userId;
    }
}
