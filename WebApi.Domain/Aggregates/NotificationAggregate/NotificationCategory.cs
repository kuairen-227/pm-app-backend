using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.NotificationAggregate;

public sealed class NotificationCategory : ValueObject
{
    public Category Value { get; }

    private NotificationCategory(Category value)
    {
        Value = value;
    }

    public static NotificationCategory Create(Category value) => new NotificationCategory(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public enum Category
    {
        // User
        UserRegistered,
        UserRoleChanged,

        // Project
        ProjectMemberInvited,
        ProjectMemberRoleChanged,

        // Ticket
        TicketCreated,
        TicketUpdated,
    }
}
