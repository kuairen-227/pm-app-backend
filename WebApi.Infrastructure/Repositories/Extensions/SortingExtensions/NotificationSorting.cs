using System.Linq.Expressions;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Common;

namespace WebApi.Infrastructure.Repositories.Extensions.SortingExtensions;

public static class NotificationSorting
{
    private static readonly Dictionary<string, Expression<Func<Notification, object>>> SortMap;
    static NotificationSorting()
    {
        var notificationMap = new Dictionary<string, Expression<Func<Notification, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["recipientId"] = t => t.RecipientId,
            ["category"] = t => t.Category.Value,
            ["subjectId"] = t => t.SubjectId,
            ["message"] = t => t.Message,
            ["isRead"] = t => t.IsRead,
            ["createdBy"] = t => t.AuditInfo.CreatedBy,
            ["createdAt"] = t => t.AuditInfo.CreatedAt,
            ["updatedBy"] = t => t.AuditInfo.UpdatedBy,
            ["updatedAt"] = t => t.AuditInfo.UpdatedAt
        };

        var auditMap = SortingHelper.CreateAuditSortMap<Notification>();
        SortMap = SortingHelper.Merge(notificationMap, auditMap);
    }

    public static IQueryable<Notification> ApplyNotificationSorting(
        this IQueryable<Notification> query,
        string? sortBy,
        SortOrder sortOrder)
    {
        return query.ApplySorting(
            sortBy,
            sortOrder,
            SortMap,
            t => t.AuditInfo.UpdatedAt);
    }
}
