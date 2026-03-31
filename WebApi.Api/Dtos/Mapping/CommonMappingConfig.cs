using Mapster;
using WebApi.Api.Dtos.Request.Common;
using WebApi.Application.Common.Pagination;

namespace WebApi.Api.Dtos.Mapping;

/// <summary>
/// Common Mapping
/// </summary>
public class CommonMappingConfig : IRegister
{
    /// <summary>
    /// Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        // Request DTO → Pagination Query
        config.NewConfig<PagedListRequest, PaginationOptions>()
            .Map(dest => dest.PageNumber, src => src.PageNumber)
            .Map(dest => dest.PageSize, src => src.PageSize);

        config.NewConfig<PagedListRequest, SortingOptions>()
            .Map(dest => dest.SortBy, src => src.SortBy)
            .Map(dest => dest.SortOrder, src => src.SortOrder);
    }
}
