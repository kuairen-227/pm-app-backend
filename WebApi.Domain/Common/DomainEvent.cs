using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Common;

public abstract class DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; }

    protected DomainEvent(IDateTimeProvider clock)
    {
        OccurredAt = clock.Now;
    }
}
