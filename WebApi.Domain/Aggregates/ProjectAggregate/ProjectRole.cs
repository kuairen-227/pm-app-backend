using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class ProjectRole : ValueObject
{
    public enum RoleType { ProjectManager, Member }
    public RoleType Value { get; }

    private ProjectRole(RoleType value)
    {
        Value = value;
    }

    public static ProjectRole Create(RoleType value) => new ProjectRole(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
