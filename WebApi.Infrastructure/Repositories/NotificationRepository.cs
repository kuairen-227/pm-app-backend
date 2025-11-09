using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Common;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Repositories.Extensions;

namespace WebApi.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _dbContext;

    public NotificationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<Notification>> ListByRecipientIdAsync(
        Guid recipientId,
        int skip = 0,
        int take = 20,
        string? sortBy = null,
        SortOrder? sortOrder = SortOrder.Desc,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Notification> query = _dbContext.Notifications
            .AsNoTracking()
            .Where(n => n.RecipientId == recipientId);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .ApplyPaging(skip, take)
            .ApplySorting(sortBy, sortOrder)
            .ToListAsync(cancellationToken);

        return new PagedResult<Notification>(items, totalCount);
    }

    public async Task<Notification?> GetByIdAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Notifications
            .FirstOrDefaultAsync(u => u.Id == notificationId, cancellationToken = default);
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        await _dbContext.Notifications.AddAsync(notification, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken cancellationToken = default)
    {
        await _dbContext.Notifications.AddRangeAsync(notifications, cancellationToken);
    }

    public Task DeleteAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        _dbContext.Notifications.Remove(notification);
        return Task.CompletedTask;
    }
}
