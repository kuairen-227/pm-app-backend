using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public Guid CreatedBy { get; }
    public DateTime CreatedAt { get; }
    public Guid UpdatedBy { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity() { } // EF Core 用

    protected Entity(Guid createdBy, IDateTimeProvider clock)
    {
        CreatedBy = createdBy;
        CreatedAt = clock.Now;
        UpdatedBy = createdBy;
        UpdatedAt = clock.Now;
    }

    public void UpdateAuditInfo(Guid updatedBy, IDateTimeProvider clock)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = clock.Now;
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
