using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class User : Entity
{
    public string Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Role Role { get; private set; } = null!;

    private readonly List<Notification> _notifications = new();
    public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();

    private User() { } // EF Core 用

    public User(string name, Email email, Role role, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("USER_NAME_REQUIRED", "Name は必須です");

        Name = name;
        Email = email;
        Role = role;
    }

    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
    }
}
