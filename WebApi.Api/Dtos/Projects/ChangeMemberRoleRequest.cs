using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Projects;

/// <summary>
/// プロジェクトメンバーロール変更リクエストDTO
/// </summary>
public class ChangeMemberRoleRequest
{
    /// <summary>
    /// プロジェクトロール
    /// </summary>
    [Required]
    public string ProjectRole { get; set; } = default!;
}
