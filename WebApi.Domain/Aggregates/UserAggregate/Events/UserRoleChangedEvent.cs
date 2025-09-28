using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Aggregates.UserAggregate.Events;

public sealed class UserRoleChangedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid UserId { get; }
    public SystemRole.RoleType Role { get; }
    public DateTime OccurredAt { get; }

    public UserRoleChangedEvent(
        Guid userId, SystemRole.RoleType role, IDateTimeProvider clock)
    {
        UserId = userId;
        Role = role;
        OccurredAt = clock.Now;
    }
}
