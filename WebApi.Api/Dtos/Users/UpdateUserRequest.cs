using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Users;

/// <summary>
/// ユーザー編集リクエストDTO
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// 名前
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// メールアドレス
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// パスワード
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// システムロール
    /// </summary>
    public string? Role { get; set; }
}
