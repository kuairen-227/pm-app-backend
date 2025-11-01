using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class SystemRole : ValueObject
{
    public enum RoleType { Admin, User }
    public RoleType Value { get; }

    private SystemRole() { } // EF Core 用

    private SystemRole(RoleType value)
    {
        Value = value;
    }

    public static SystemRole Create(RoleType value) => new SystemRole(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
