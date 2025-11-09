namespace WebApi.Domain.Common;

public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount);
