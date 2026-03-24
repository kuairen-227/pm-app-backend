using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Tickets;

/// <summary>
/// チケット詳細レスポンスDTO
/// </summary>
public class TicketDetailResponse : AuditInfoResponse
{
    /// <summary>
    /// チケットID
    /// </summary>
    [Required]
    public Guid Id { get; set; }

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

    /// <summary>
    /// チケット完了条件
    /// </summary>
    [Required]
    public IReadOnlyList<TicketCompletionCriterionResponse> CompletionCriteria { get; set; } = [];

    /// <summary>
    /// チケットコメント
    /// </summary>
    [Required]
    public IReadOnlyList<TicketCommentResponse> Comments { get; set; } = [];

    /// <summary>
    /// チケット履歴
    /// </summary>
    [Required]
    public IReadOnlyList<TicketHistoryResponse> Histories { get; set; } = [];
}
