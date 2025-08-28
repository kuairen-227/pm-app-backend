using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Tests.Helpers;

public class NotificationBuilder
{
    private Guid _userId = Guid.NewGuid();
    private string _message = "デフォルトメッセージ";
    private bool _isRead = false;

    public NotificationBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public NotificationBuilder WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public NotificationBuilder MarkAsRead()
    {
        _isRead = true;
        return this;
    }

    public Notification Build()
    {
        var notification = new Notification(
            _userId,
            _message
        );

        if (_isRead)
        {
            notification.MarkAsRead();
        }

        return notification;
    }
}
