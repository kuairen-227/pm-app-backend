using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Commands.Tickets.AssignUser;

[RequiresPermission(TicketPermissions.Assign)]
public class AssignUserCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Guid UserId { get; }

    public AssignUserCommand(Guid projectId, Guid ticketId, Guid userId)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        UserId = userId;
    }
}
