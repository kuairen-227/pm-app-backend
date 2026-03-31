namespace WebApi.Api.Dtos.Request.Common;

/// <summary>
/// ページネーション一覧リクエストDTO
/// </summary>
public class PagedListRequest
{
    /// <summary>
    /// ページ番号
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// ページサイズ
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// ソート対象
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// ソート順
    /// </summary>
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
}
