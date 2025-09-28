using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.NotificationAggregate;

public sealed class Notification : Entity
{
    public Guid RecipientId { get; private set; }
    public NotificationCategory Category { get; private set; } = null!;
    public Guid SubjectId { get; private set; }
    public string Message { get; private set; } = null!;
    public bool IsRead { get; private set; }

    private Notification() { } // EF Core 用

    public Notification(
        Guid recipientId,
        NotificationCategory.Category category,
        Guid subjectId,
        string message,
        Guid createdBy,
        IDateTimeProvider clock
    ) : base(createdBy, clock)
    {
        if (recipientId == Guid.Empty)
            throw new DomainException("NOTIFICATION_RECIPIENT_ID_REQUIRED", "RecipientId は必須です");
        if (subjectId == Guid.Empty)
            throw new DomainException("NOTIFICATION_SUBJECT_ID_REQUIRED", "SubjectId は必須です");
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("NOTIFICATION_MESSAGE_REQUIRED", "Message は必須です");

        RecipientId = recipientId;
        Category = NotificationCategory.Create(category);
        SubjectId = subjectId;
        Message = message;
        IsRead = false;
    }

    public void MarkAsRead() => IsRead = true;
}
