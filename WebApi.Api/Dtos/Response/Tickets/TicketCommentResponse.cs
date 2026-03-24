using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Response.Tickets;

/// <summary>
/// チケットコメントレスポンスDTO
/// </summary>
public class TicketCommentResponse
{
    /// <summary>
    /// 投稿者ID
    /// </summary>
    [Required]
    public Guid AuthorId { get; set; }

    /// <summary>
    /// コメント内容
    /// </summary>
    [Required]
    public string Content { get; set; } = default!;
}
