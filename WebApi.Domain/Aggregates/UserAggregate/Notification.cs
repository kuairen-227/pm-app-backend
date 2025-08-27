using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class Notification : Entity
{
    public Guid UserId { get; private set; }
    public string Message { get; private set; }
    public bool IsRead { get; private set; }

    public Notification(Guid userId, string message)
    {
        if (userId == Guid.Empty)
            throw new DomainException("NOTIFICATION_USER_ID_REQUIRED", "UserId は必須です");
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("NOTIFICATION_MESSAGE_REQUIRED", "Message は必須です");

        UserId = userId;
        Message = message;
        IsRead = false;
    }

    public void MarkAsRead() => IsRead = true;
}
