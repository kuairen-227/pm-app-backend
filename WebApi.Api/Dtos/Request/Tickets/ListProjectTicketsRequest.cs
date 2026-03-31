using WebApi.Api.Dtos.Request.Common;

namespace WebApi.Api.Dtos.Request.Tickets;

/// <summary>
/// チケット一覧取得リクエストDTO
/// </summary>
public class ListProjectTicketsRequest : PagedListRequest
{
    /// <summary>
    /// タイトル
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 担当者ID
    /// </summary>
    public Guid? AssigneeId { get; set; }

    /// <summary>
    /// ステータス
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 開始日From
    /// </summary>
    public DateOnly? StartDateFrom { get; set; }

    /// <summary>
    /// 開始日To
    /// </summary>
    public DateOnly? StartDateTo { get; set; }

    /// <summary>
    /// 終了日From
    /// </summary>
    public DateOnly? EndDateFrom { get; set; }

    /// <summary>
    /// 終了日To
    /// </summary>
    public DateOnly? EndDateTo { get; set; }
}
