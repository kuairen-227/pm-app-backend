using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.NotificationAggregate;

public sealed class Notification : Entity
{
    public Guid RecipientId { get; private set; }
    public NotificationCategory Category { get; private set; } = null!;
    public string Message { get; private set; } = null!;
    public bool IsRead { get; private set; }

    private Notification() { } // EF Core 用

    public Notification(Guid recipientId, NotificationCategory category, string message, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (recipientId == Guid.Empty)
            throw new DomainException("NOTIFICATION_RECIPIENT_ID_REQUIRED", "RecipientId は必須です");
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("NOTIFICATION_MESSAGE_REQUIRED", "Message は必須です");

        RecipientId = recipientId;
        Category = category;
        Message = message;
        IsRead = false;
    }

    public void MarkAsRead() => IsRead = true;
}
