namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケットコメント編集リクエストDTO
/// </summary>
public class EditTicketCommentRequest
{
    /// <summary>
    /// コメント内容
    /// </summary>
    public string Content { get; set; } = default!;
}
