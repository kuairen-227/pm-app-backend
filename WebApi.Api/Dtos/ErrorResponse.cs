namespace WebApi.Api.Dtos;

/// <summary>
/// エラーレスポンスDTO
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// エラー詳細
    /// </summary>
    public ErrorDetail Error { get; set; } = default!;
}

/// <summary>
/// エラー詳細DTO
/// </summary>
public class ErrorDetail
{
    /// <summary>
    /// エラーコード
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    /// エラーメッセージ
    /// </summary>
    public string Message { get; set; } = default!;

    /// <summary>トレースID</summary>
    /// <remarks>
    /// ログとの紐付けに使用
    /// </remarks>
    public string TraceId { get; set; } = default!;
}
