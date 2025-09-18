using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Commands.Tickets.AssignUser;

[RequiresPermission(TicketPermissions.Assign)]
public class AssignUserCommand : IRequest<Unit>
{
    public Guid Id { get; }
    public Guid UserId { get; }

    public AssignUserCommand(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }
}
