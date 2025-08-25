using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.UserAggregate;

public sealed class Notification : Entity
{
    public Guid UserId { get; private set; }
    public string Message { get; private set; }
    public bool IsRead { get; private set; }

    public Notification(Guid userId, string message)
    {
        UserId = userId;
        Message = message ?? throw new ArgumentNullException(nameof(message));
        IsRead = false;
    }

    public void MarkAsRead() => IsRead = true;
}
