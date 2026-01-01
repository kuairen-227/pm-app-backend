namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケット完了条件編集リクエストDTO
/// </summary>
public class EditCompletionCriterionRequest
{
    /// <summary>
    /// 完了条件
    /// </summary>
    public string Criterion { get; set; } = default!;
}
