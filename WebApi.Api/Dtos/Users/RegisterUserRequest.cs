using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Users;

/// <summary>
/// ユーザー登録リクエストDTO
/// </summary>
public class RegisterUserRequest
{
    /// <summary>
    /// 名前
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    /// メールアドレス
    /// </summary>
    [Required]
    public string Email { get; set; } = default!;

    /// <summary>
    /// システムロール
    /// </summary>
    public string Role { get; set; } = default!;
}
