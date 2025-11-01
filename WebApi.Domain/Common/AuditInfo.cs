using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Common;

public class AuditInfo
{
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid UpdatedBy { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public AuditInfo(Guid createdBy, IDateTimeProvider clock)
    {
        CreatedBy = createdBy;
        CreatedAt = clock.Now;
        UpdatedBy = createdBy;
        UpdatedAt = clock.Now;
    }

    public void Touch(Guid updatedBy, IDateTimeProvider clock)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = clock.Now;
    }
}
