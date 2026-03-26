using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Tickets;

/// <summary>
/// チケットレスポンスDTO
/// </summary>
public class TicketResponse : AuditInfoResponse
{
    /// <summary>
    /// チケットID
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// プロジェクトID
    /// </summary>
    [Required]
    public Guid ProjectId { get; set; }

    /// <summary>
    /// チケットタイトル
    /// </summary>
    [Required]
    public string Title { get; set; } = default!;

    /// <summary>
    /// 担当者ID
    /// </summary>
    public Guid? AssigneeId { get; set; }

    /// <summary>
    /// 開始日
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// 終了日
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// ステータス
    /// </summary>
    [Required]
    public string Status { get; set; } = default!;
}
