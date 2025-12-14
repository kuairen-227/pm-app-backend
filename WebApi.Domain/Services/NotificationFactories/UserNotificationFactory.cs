using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Services.NotificationFactories;

public sealed class UserNotificationFactory
{
    public readonly IDateTimeProvider _clock;

    public UserNotificationFactory(IDateTimeProvider clock)
    {
        _clock = clock;
    }

    public Notification CreateUserRegisteredNotification(
        Guid recipientId, Guid userId, string userName, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.UserRegistered,
            userId,
            $"{userName} が登録されました。",
            createdBy,
            _clock
        );
    }

    public Notification CreateUserUpdatedNotification(Guid recipientId, Guid userId, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.UserUpdated,
            userId,
            $"ユーザー情報が更新されました。",
            createdBy,
            _clock
        );
    }
}
