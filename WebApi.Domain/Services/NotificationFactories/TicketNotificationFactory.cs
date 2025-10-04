using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Domain.Services.NotificationFactories;

public sealed class TicketNotificationFactory
{
    public readonly IDateTimeProvider _clock;

    public TicketNotificationFactory(IDateTimeProvider clock)
    {
        _clock = clock;
    }

    public Notification CreateForTicketCreation(
        Guid recipientId, Guid ticketId, TicketTitle ticketTitle, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.TicketCreated,
            ticketId,
            $"チケット {ticketTitle} が作成されました。",
            createdBy,
            _clock
        );
    }

    public Notification CreateForTicketUpdate(
        Guid recipientId, Guid ticketId, string ticketTitle, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.TicketUpdated,
            ticketId,
            $"チケット {ticketTitle} が更新されました。",
            createdBy,
            _clock
        );
    }

    public Notification CreateForCommentAdded(
        Guid recipientId, Guid ticketId, string ticketTitle, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.TicketCommentAdded,
            ticketId,
            $"チケット {ticketTitle} にコメントが追加されました。",
            createdBy,
            _clock
        );
    }
}
