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

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("USER_NAME_REQUIRED", "Name は必須です");

        Name = name;
        UpdateAuditInfo(Id);
        AddDomainEvent(new UserUpdatedEvent(Id, _clock));
    }

    public void ChangeEmail(string newEmail)
    {
        Email = Email.Create(newEmail);
        UpdateAuditInfo(Id);
        AddDomainEvent(new UserUpdatedEvent(Id, _clock));
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("USER_PASSWORD_REQUIRED", "PasswordHash は必須です");

        PasswordHash = newPasswordHash;
        UpdateAuditInfo(Id);
    }

    public void ChangeUserRole(SystemRole.RoleType newRole)
    {
        Role = SystemRole.Create(newRole);
        UpdateAuditInfo(Id);
        AddDomainEvent(new UserUpdatedEvent(Id, _clock));
    }
}
