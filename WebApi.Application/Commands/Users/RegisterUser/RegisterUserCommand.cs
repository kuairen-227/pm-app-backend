using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Users.RegisterUser;

[RequiresPermission(UserPermissions.Manage)]
public class RegisterUserCommand : IRequest<Guid>
{
    public string Name { get; }
    public string Email { get; }
    public string Password { get; }
    public SystemRole.RoleType Role { get; }

    public RegisterUserCommand(
        string name, string email, string password, SystemRole.RoleType role)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }
}
