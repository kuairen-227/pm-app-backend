using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class NotificationBuilder : BaseBuilder<NotificationBuilder, Notification>
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

    public override Notification Build()
    {
        var notification = new Notification(
            _userId,
            _message,
            _createdBy,
            _clock
        );

        if (_isRead)
        {
            notification.MarkAsRead();
        }

        return notification;
    }
}
