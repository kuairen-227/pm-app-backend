using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class Role : ValueObject
{
    public enum RoleType { Admin, Manager, Member }
    public RoleType Value { get; }

    private Role(RoleType value)
    {
        Value = value;
    }

    public static Role Create(RoleType value) => new Role(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
