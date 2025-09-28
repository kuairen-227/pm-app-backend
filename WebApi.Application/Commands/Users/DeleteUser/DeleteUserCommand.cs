using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Users.DeleteUser;

[RequiresPermission(UserPermissions.Manage)]
public class DeleteUserCommand : IRequest<Unit>
{
    public Guid UserId { get; }

    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }
}
