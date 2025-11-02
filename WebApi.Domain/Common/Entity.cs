using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Common;

public abstract class Entity : IHasDomainEvents
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public AuditInfo AuditInfo { get; protected set; } = null!;

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected readonly IDateTimeProvider _clock = null!;

    protected Entity() { } // EF Core 用

    protected Entity(Guid createdBy, IDateTimeProvider clock)
    {
        AuditInfo = new AuditInfo(createdBy, clock);
        _clock = clock;
    }

    public void UpdateAuditInfo(Guid updatedBy)
    {
        AuditInfo.Touch(updatedBy, _clock);
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
