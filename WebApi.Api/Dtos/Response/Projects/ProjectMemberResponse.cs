using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Response.Projects;

/// <summary>
/// プロジェクトメンバーレスポンスDTO
/// </summary>
public class ProjectMemberResponse
{
    /// <summary>
    /// ユーザーID
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// プロジェクトロール
    /// </summary>
    [Required]
    public string ProjectRole { get; set; } = default!;
}
