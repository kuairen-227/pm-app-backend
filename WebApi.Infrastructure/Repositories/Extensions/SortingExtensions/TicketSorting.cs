using System.Linq.Expressions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Infrastructure.Repositories.Extensions.SortingExtensions;

public static class TicketSorting
{
    private static readonly Dictionary<string, Expression<Func<Ticket, object>>> SortMap;
    static TicketSorting()
    {
        var ticketMap = new Dictionary<string, Expression<Func<Ticket, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["projectId"] = t => t.ProjectId,
            ["title"] = t => t.Title.Value,
            ["description"] = t => t.Description.Value,
            ["assigneeId"] = t => t.AssigneeId ?? Guid.Empty,
            ["startDate"] = t => t.Schedule.StartDate ?? DateOnly.MinValue,
            ["endDate"] = t => t.Schedule.EndDate ?? DateOnly.MinValue,
            ["status"] = t => t.Status.Value,
            ["createdBy"] = t => t.AuditInfo.CreatedBy,
            ["createdAt"] = t => t.AuditInfo.CreatedAt,
            ["updatedBy"] = t => t.AuditInfo.UpdatedBy,
            ["updatedAt"] = t => t.AuditInfo.UpdatedAt
        };

        var auditMap = SortingHelper.CreateAuditSortMap<Ticket>();
        SortMap = SortingHelper.Merge(ticketMap, auditMap);
    }

    public static IQueryable<Ticket> ApplyTicketSorting(
        this IQueryable<Ticket> query,
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
