using WebApi.Domain.Common;

namespace WebApi.Application.Common.Pagination;

public sealed class SortingOptions
{
    public string SortBy { get; init; } = "UpdateAt";
    public SortOrder SortOrder { get; init; } = SortOrder.Desc;
}
