namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// チケット完了条件追加リクエストDTO
/// </summary>
public class AddCompletionCriterionRequest
{
    /// <summary>
    /// 完了条件
    /// </summary>
    public string Criterion { get; set; } = default!;
}
