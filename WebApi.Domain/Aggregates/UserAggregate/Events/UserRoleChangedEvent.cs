using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate.Events;

public sealed class UserRoleChangedEvent : DomainEvent
{
    public Guid UserId { get; }
    public SystemRole.RoleType Role { get; }

    public UserRoleChangedEvent(
        Guid userId, SystemRole.RoleType role, IDateTimeProvider clock
    ) : base(clock)
    {
        UserId = userId;
        Role = role;
    }
}
