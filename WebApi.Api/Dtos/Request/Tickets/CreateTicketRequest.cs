using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Request.Tickets;

/// <summary>
/// チケット作成リクエストDTO
/// </summary>
public class CreateTicketRequest
{
    /// <summary>
    /// チケットタイトル
    /// </summary>
    [Required]
    public string Title { get; set; } = default!;

    /// <summary>
    /// チケット説明
    /// </summary>
    [Required]
    public string Description { get; set; } = default!;

    /// <summary>
    /// 担当者
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
    /// 完了条件
    /// </summary>
    public IReadOnlyCollection<string>? CompletionCriteria { get; set; }

    /// <summary>
    /// 通知対象ユーザー
    /// </summary>
    public required IReadOnlyCollection<Guid> NotificationRecipientIds { get; set; }
}
