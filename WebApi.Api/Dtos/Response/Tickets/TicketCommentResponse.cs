using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Tickets;

/// <summary>
/// チケットコメントレスポンスDTO
/// </summary>
public class TicketCommentResponse : AuditInfoResponse
{
    /// <summary>
    /// チケットコメントID
    /// </summary>
    [Required]
    public Guid Id { get; set; }

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
