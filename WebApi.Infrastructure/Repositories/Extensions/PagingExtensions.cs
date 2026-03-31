namespace WebApi.Infrastructure.Repositories.Extensions;

public static class PagingExtensions
{
    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> query,
        int skip,
        int take)
    {
        return query.Skip(skip).Take(take);
    }
}
