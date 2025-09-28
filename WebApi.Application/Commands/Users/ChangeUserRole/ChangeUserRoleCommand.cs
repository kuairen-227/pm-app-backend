using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Users.ChangeUserRole;

[RequiresPermission(UserPermissions.Manage)]
public class ChangeUserRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; }
    public SystemRole.RoleType SystemRole { get; }

    public ChangeUserRoleCommand(Guid userId, SystemRole.RoleType systemRole)
    {
        UserId = userId;
        SystemRole = systemRole;
    }
}
