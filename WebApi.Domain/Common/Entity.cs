using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public Guid CreatedBy { get; }
    public DateTimeOffset CreatedAt { get; }
    public Guid UpdatedBy { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

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
}
