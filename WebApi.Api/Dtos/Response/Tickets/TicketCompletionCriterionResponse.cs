using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Response.Tickets;

/// <summary>
/// チケット完了条件レスポンスDTO
/// </summary>
public class TicketCompletionCriterionResponse
{
    /// <summary>
    /// 完了条件
    /// </summary>
    [Required]
    public string Criterion { get; set; } = default!;

    /// <summary>
    /// 完了状況
    /// </summary>
    [Required]
    public bool IsCompleted { get; set; }
}
