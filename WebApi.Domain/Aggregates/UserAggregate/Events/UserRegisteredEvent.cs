using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Aggregates.UserAggregate.Events;

public sealed class UserRegisteredEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid UserId { get; }
    public string UserName { get; }
    public DateTime OccurredAt { get; }

    public UserRegisteredEvent(
        Guid userId, string userName, IDateTimeProvider clock)
    {
        UserId = userId;
        UserName = userName;
        OccurredAt = clock.Now;
    }
}
