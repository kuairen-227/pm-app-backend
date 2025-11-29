namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケットコメント投稿リクエストDTO
/// </summary>
public class AddTicketCommentRequest
{
    /// <summary>
    /// コメント内容
    /// </summary>
    public string Content { get; set; } = default!;

    /// <summary>
    /// 通知対象ユーザー
    /// </summary>
    public required IReadOnlyCollection<Guid> NotificationRecipientIds { get; set; }
}
