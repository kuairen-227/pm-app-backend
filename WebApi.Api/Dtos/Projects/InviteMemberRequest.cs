using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Projects;

/// <summary>
/// プロジェクトメンバー追加リクエストDTO
/// </summary>
public class InviteMemberRequest
{
    /// <summary>
    /// ユーザーID
    /// </summary>
    [Required]
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// プロジェクトロール
    /// </summary>
    [Required]
    public string ProjectRole { get; set; } = default!;
}
