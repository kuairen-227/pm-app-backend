using MediatR;
using WebApi.Application.Common.Pagination;

namespace WebApi.Application.Common.Pagination;

public abstract class PagedQuery<TResponse> : IRequest<PagedResultDto<TResponse>>
{
    public PaginationOptions Pagination { get; init; } = new();
    public SortingOptions Sorting { get; init; } = new();
}
