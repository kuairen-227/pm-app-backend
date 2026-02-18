using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.UserAggregate.Events;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class User : Entity
{
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public SystemRole Role { get; private set; } = null!;

    private User() { } // EF Core 用

    public User(
        string name, string email, string passwordHash, SystemRole.RoleType role, Guid createdBy, IDateTimeProvider clock
    ) : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("USER_NAME_REQUIRED", "Name は必須です");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("USER_PASSWORD_REQUIRED", "PasswordHash は必須です");

        Name = name;
        Email = Email.Create(email);
        PasswordHash = passwordHash;
        Role = SystemRole.Create(role);

        AddDomainEvent(new UserRegisteredEvent(Id, Name, clock));
    }

    public void ChangeName(string name, IDateTimeProvider clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("USER_NAME_REQUIRED", "Name は必須です");

        Name = name;
        UpdateAuditInfo(Id, clock);
        AddDomainEvent(new UserUpdatedEvent(Id, clock));
    }

    public void ChangeEmail(string newEmail, IDateTimeProvider clock)
    {
        Email = Email.Create(newEmail);
        UpdateAuditInfo(Id, clock);
        AddDomainEvent(new UserUpdatedEvent(Id, clock));
    }

    public void ChangePassword(string newPasswordHash, IDateTimeProvider clock)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("USER_PASSWORD_REQUIRED", "PasswordHash は必須です");

        PasswordHash = newPasswordHash;
        UpdateAuditInfo(Id, clock);
    }

    public void ChangeUserRole(SystemRole.RoleType newRole, IDateTimeProvider clock)
    {
        Role = SystemRole.Create(newRole);
        UpdateAuditInfo(Id, clock);
        AddDomainEvent(new UserUpdatedEvent(Id, clock));
    }
}
