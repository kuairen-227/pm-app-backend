namespace WebApi.Api.Dtos.Auth;

/// <summary>
/// ログアウトリクエストDTO
/// </summary>
public class LogoutRequest
{
    /// <summary>
    /// リフレッシュトークン
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}
