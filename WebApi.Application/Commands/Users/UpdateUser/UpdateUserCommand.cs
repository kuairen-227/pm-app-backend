using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Users.UpdateUser;

[RequiresPermission(UserPermissions.Manage)]
public class UpdateUserCommand : IRequest<Unit>
{
    public Guid UserId { get; }
    public string? Name { get; }
    public string? Email { get; }
    public string? Password { get; }
    public SystemRole.RoleType? Role { get; }

    public UpdateUserCommand(
        Guid userId, string? name, string? email, string? password, SystemRole.RoleType? systemRole)
    {
        UserId = userId;
        Name = name;
        Email = email;
        Password = password;
        Role = systemRole;
    }
}
