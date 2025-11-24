using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケット編集リクエストDTO
/// </summary>
public class UpdateTicketRequest
{
    /// <summary>
    /// タイトル
    /// </summary>
    [Required]
    public string Title { get; set; } = default!;

    /// <summary>
    /// 通知対象ユーザー
    /// </summary>
    public required IReadOnlyCollection<Guid> NotificationRecipientIds { get; set; }
}
