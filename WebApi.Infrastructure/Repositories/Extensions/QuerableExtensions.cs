using System.Linq.Expressions;
using System.Reflection;
using WebApi.Domain.Common;

namespace WebApi.Infrastructure.Repositories.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> query,
        int skip,
        int take)
    {
        return query.Skip(skip).Take(take);
    }

    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        string? sortBy = null,
        SortOrder? sortOrder = SortOrder.Desc)
    {
        if (string.IsNullOrWhiteSpace(sortBy)) return query;

        var property = typeof(T).GetProperty(sortBy, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (property == null) return query;

        var param = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(param, property);
        var orderByExp = Expression.Lambda(propertyAccess, param);

        string methodName = sortOrder == SortOrder.Desc ? "OrderByDescending" : "OrderBy";

        var resultExp = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), property.PropertyType],
            query.Expression,
            Expression.Quote(orderByExp)
        );

        return query.Provider.CreateQuery<T>(resultExp);
    }
}
