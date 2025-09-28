using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Services.NotificationFactories;

public sealed class UserNotificationFactory
{
    public readonly IDateTimeProvider _clock;

    public UserNotificationFactory(IDateTimeProvider clock)
    {
        _clock = clock;
    }
}
