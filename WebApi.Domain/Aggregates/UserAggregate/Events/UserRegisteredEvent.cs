using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate.Events;

public sealed class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; }
    public string UserName { get; }

    public UserRegisteredEvent(
        Guid userId, string userName, IDateTimeProvider clock
    ) : base(clock)
    {
        UserId = userId;
        UserName = userName;
    }
}
