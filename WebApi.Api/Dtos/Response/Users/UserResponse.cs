using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Users;

/// <summary>
/// ユーザーレスポンスDTO
/// </summary>
public class UserResponse : AuditInfoResponse
{
    /// <summary>
    /// ユーザーID
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// ユーザー名
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    /// Eメール
    /// </summary>
    [Required]
    public string Email { get; set; } = default!;

    /// <summary>
    /// システムロール
    /// </summary>
    [Required]
    public string Role { get; set; } = default!;
}
