using System.ComponentModel.DataAnnotations;
using WebApi.Api.Dtos.Common;

namespace WebApi.Api.Dtos.Response.Projects;

/// <summary>
/// プロジェクト詳細レスポンスDTO
/// </summary>
public class ProjectDetailResponse : AuditInfoResponse
{
    /// <summary>
    /// プロジェクトID
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// プロジェクト名
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    /// プロジェクト説明
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// プロジェクトメンバー
    /// </summary>
    [Required]
    public IReadOnlyList<ProjectMemberResponse> Members { get; set; } = [];
}
