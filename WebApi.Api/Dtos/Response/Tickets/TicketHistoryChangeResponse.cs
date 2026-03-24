using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Response.Tickets;

/// <summary>
/// チケット履歴変更内容レスポンスDTO
/// </summary>
public class TicketHistoryChangeResponse
{
    /// <summary>
    /// 変更項目
    /// </summary>
    [Required]
    public string Field { get; set; } = default!;

    /// <summary>
    /// 変更前
    /// </summary>
    public string? Before { get; set; }

    /// <summary>
    /// 変更後
    /// </summary>
    public string? After { get; set; }
}
