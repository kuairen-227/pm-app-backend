using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class NotificationBuilder : BaseBuilder<NotificationBuilder, Notification>
{
    private Guid _recipientId = Guid.NewGuid();
    private NotificationCategory _category = NotificationCategory.Create(NotificationCategory.Category.ProjectMemberInvited);
    private Guid _subjectId = Guid.NewGuid();
    private string _message = "デフォルトメッセージ";
    private bool _isRead = false;

    public NotificationBuilder WithRecipientId(Guid recipientId)
    {
        _recipientId = recipientId;
        return this;
    }

    public NotificationBuilder WithCategory(NotificationCategory.Category category)
    {
        _category = NotificationCategory.Create(category);
        return this;
    }

    public NotificationBuilder WithSubjectId(Guid subjectId)
    {
        _subjectId = subjectId;
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
            _recipientId,
            _category.Value,
            _subjectId,
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
