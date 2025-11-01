using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class ProjectMember : ValueObject
{
    public Guid UserId { get; }
    public ProjectRole Role { get; } = null!;

    private ProjectMember() { } // EF Core 用

    public ProjectMember(Guid userId, ProjectRole role)
    {
        if (userId == Guid.Empty)
            throw new DomainException("USER_ID_REQUIRED", "UserId は必須です");

        UserId = userId;
        Role = role;
    }

    public static ProjectMember Create(Guid userId, ProjectRole role) => new ProjectMember(userId, role);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return UserId;
        yield return Role;
    }
}
