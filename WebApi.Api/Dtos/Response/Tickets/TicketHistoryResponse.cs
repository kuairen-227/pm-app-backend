using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Tickets;

/// <summary>
/// チケット履歴レスポンスDTO
/// </summary>
public class TicketHistoryResponse : AuditInfoResponse
{
    /// <summary>
    /// チケット履歴ID
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// 実行者ID
    /// </summary>
    [Required]
    public Guid ActorId { get; set; }

    /// <summary>
    /// 発生日時
    /// </summary>
    [Required]
    public DateTime OccurredAt { get; set; }

    /// <summary>
    /// 実行内容
    /// </summary>
    [Required]
    public string Action { get; set; } = default!;

    /// <summary>
    /// チケット履歴変更内容
    /// </summary>
    [Required]
    public IReadOnlyList<TicketHistoryChangeResponse> Changes { get; set; } = [];
}
