using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate.Events;

public sealed class UserUpdatedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserUpdatedEvent(Guid userId, IDateTimeProvider clock) : base(clock)
    {
        UserId = userId;
    }
}
