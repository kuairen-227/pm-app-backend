using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Response.Common;

/// <summary>
/// ページネーションレスポンスDTO
/// </summary>
public class PaginatedResponse<T>
{
    /// <summary>
    /// ページネーションアイテム
    /// </summary>
    [Required]
    public IReadOnlyList<T> Items { get; set; } = [];

    /// <summary>
    /// トータルカウント
    /// </summary>
    [Required]
    public int TotalCount { get; set; }

    /// <summary>
    /// ページ番号
    /// </summary>
    [Required]
    public int PageNumber { get; set; }

    /// <summary>
    /// ページサイズ
    /// </summary>
    [Required]
    public int PageSize { get; set; }

    /// <summary>
    /// トータルページ数
    /// </summary>
    [Required]
    public int TotalPages { get; set; }
}
