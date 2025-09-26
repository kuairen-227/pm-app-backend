using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.NotificationAggregate;

namespace WebApi.Domain.Services;

public sealed class NotificationFactory
{
    public readonly IDateTimeProvider _clock;

    public NotificationFactory(IDateTimeProvider clock)
    {
        _clock = clock;
    }

    public Notification CreateForProjectInvitation(Guid recipientId, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Create(NotificationCategory.Category.ProjectInvitation),
            "プロジェクトに招待されました。",
            createdBy,
            _clock
        );
    }

    public Notification CreateForProjectChangeMemberRole(Guid recipientId, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Create(NotificationCategory.Category.ProjectChangeMemberRole),
            "メンバー権限が変更されました。",
            createdBy,
            _clock
        );
    }
}
