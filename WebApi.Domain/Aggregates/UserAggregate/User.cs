using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class User : Entity
{
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public SystemRole Role { get; private set; } = null!;

    private User() { } // EF Core 用

    public User(string name, string email, SystemRole.RoleType role, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("USER_NAME_REQUIRED", "Name は必須です");

        Name = name;
        Email = Email.Create(email);
        Role = SystemRole.Create(role);
    }
}
