using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケット作成リクエストDTO
/// </summary>
public class CreateTicketRequest
{
    /// <summary>
    /// タイトル
    /// </summary>
    [Required]
    public string Title { get; set; } = default!;

    /// <summary>
    /// 担当者
    /// </summary>
    public Guid? AssigneeId { get; set; }

    /// <summary>
    /// 期限日
    /// </summary>
    public DateOnly? Deadline { get; set; }

    /// <summary>
    /// 完了条件
    /// </summary>
    public string? CompletionCriteria { get; set; }

    /// <summary>
    /// 通知対象ユーザー
    /// </summary>
    public required IReadOnlyCollection<Guid> NotificationRecipientIds { get; set; }
}
