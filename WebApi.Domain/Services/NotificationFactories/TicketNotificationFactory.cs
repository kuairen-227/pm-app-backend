using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Services.NotificationFactories;

public sealed class TicketNotificationFactory
{
    public readonly IDateTimeProvider _clock;

    public TicketNotificationFactory(IDateTimeProvider clock)
    {
        _clock = clock;
    }
}
