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

    public Notification CreateTicketCreatedNotification(
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

    public Notification CreateTicketUpdatedNotification(
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

    public Notification CreateMemberAssignedNotification(
        Guid recipientId, Guid ticketId, string ticketTitle, string assigneeName, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.TicketMemberAssigned,
            ticketId,
            $"{assigneeName} が チケット {ticketTitle} の担当になりました。",
            createdBy,
            _clock
        );
    }

    public Notification CreateCommentAddedNotification(
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
