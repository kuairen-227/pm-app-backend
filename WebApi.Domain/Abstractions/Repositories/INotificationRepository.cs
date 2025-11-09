using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Abstractions.Repositories;

public interface INotificationRepository
{
    Task<PagedResult<Notification>> ListByRecipientIdAsync(
        Guid recipientId,
        int skip = 0,
        int take = 20,
        string? sortBy = null,
        SortOrder? sortOrder = SortOrder.Desc,
        CancellationToken cancellationToken = default);
    Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken = default);
    Task DeleteAsync(Notification notification, CancellationToken cancellationToken = default);
}
