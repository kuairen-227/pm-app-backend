using WebApi.Domain.Aggregates.NotificationAggregate;

namespace WebApi.Domain.Abstractions.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> ListByRecipientAsync(Guid recipientId, CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task DeleteAsync(Notification notification, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken = default);
}
