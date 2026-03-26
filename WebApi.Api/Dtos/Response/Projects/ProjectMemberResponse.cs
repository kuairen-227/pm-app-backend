using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Projects;

/// <summary>
/// プロジェクトメンバーレスポンスDTO
/// </summary>
public class ProjectMemberResponse : AuditInfoResponse
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
