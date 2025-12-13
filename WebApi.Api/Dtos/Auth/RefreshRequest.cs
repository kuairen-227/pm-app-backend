namespace WebApi.Api.Dtos.Auth;

/// <summary>
/// リフレッシュリクエストDTO
/// </summary>
public class RefreshRequest
{
    /// <summary>
    /// リフレッシュトークン
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}
