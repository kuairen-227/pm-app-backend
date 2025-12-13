namespace WebApi.Api.Dtos.Auth;

/// <summary>
/// ログインレスポンスDTO
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// ユーザーID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// アクセストークン
    /// </summary>
    public string AccessToken { get; set; } = default!;

    /// <summary>
    /// リフレッシュトークン
    /// </summary>
    public string RefreshToken { get; set; } = default!;
}
