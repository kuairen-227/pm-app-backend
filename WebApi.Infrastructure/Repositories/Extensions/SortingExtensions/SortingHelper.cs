using System.Linq.Expressions;
using WebApi.Domain.Common;

namespace WebApi.Infrastructure.Repositories.Extensions.SortingExtensions;

public static class SortingHelper
{
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        string? sortBy,
        SortOrder sortOrder,
        Dictionary<string, Expression<Func<T, object>>> sortMap,
        Expression<Func<T, object>> defaultSort)
    {
        if (sortBy == null || !sortMap.TryGetValue(sortBy, out var expression))
            expression = defaultSort;

        return sortOrder == SortOrder.Desc
            ? query.OrderByDescending(expression)
            : query.OrderBy(expression);
    }

    public static Dictionary<string, Expression<Func<T, object>>> Merge<T>(
        Dictionary<string, Expression<Func<T, object>>> baseMap,
        Dictionary<string, Expression<Func<T, object>>> map)
    {
        var result = new Dictionary<string, Expression<Func<T, object>>>(baseMap, StringComparer.OrdinalIgnoreCase);

        foreach (var kv in map)
            result[kv.Key] = kv.Value;

        return result;
    }

    public static Dictionary<string, Expression<Func<T, object>>> CreateAuditSortMap<T>()
        where T : Entity
    {
        return new Dictionary<string, Expression<Func<T, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["createdBy"] = x => x.AuditInfo.CreatedBy,
            ["createdAt"] = x => x.AuditInfo.CreatedAt,
            ["updatedBy"] = x => x.AuditInfo.UpdatedBy,
            ["updatedAt"] = x => x.AuditInfo.UpdatedAt
        };
    }
}
