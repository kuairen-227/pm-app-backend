using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class ProjectMember : Entity
{
    public Guid UserId { get; }
    public ProjectRole Role { get; private set; } = null!;

    private ProjectMember() { } // EF Core 用

    public ProjectMember(Guid userId, ProjectRole role, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (userId == Guid.Empty)
            throw new DomainException("USER_ID_REQUIRED", "UserId は必須です");

        UserId = userId;
        Role = role;
    }

    public void ChangeRole(ProjectRole newRole, Guid updatedBy)
    {
        Role = newRole;
        UpdateAuditInfo(updatedBy);
    }
}
