namespace WebApi.Api.Dtos.Auth;

/// <summary>
/// ログインリクエストDTO
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// メールアドレス
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// パスワード
    /// </summary>
    public string Password { get; set; } = default!;
}
