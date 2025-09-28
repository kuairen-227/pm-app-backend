using MediatR;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Events.Users.UserRoleChanged;

public sealed class UserRoleChangedNotification : INotification
{
    public Guid UserId { get; }
    public SystemRole.RoleType Role { get; }

    public UserRoleChangedNotification(Guid userId, SystemRole.RoleType role)
    {
        UserId = userId;
        Role = role;
    }
}
