using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Auth;

/// <summary>
/// ログインユーザーレスポンスDTO
/// </summary>
public class MeResponse : AuditInfoResponse
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
