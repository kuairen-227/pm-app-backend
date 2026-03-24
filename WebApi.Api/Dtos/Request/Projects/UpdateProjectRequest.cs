using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Request.Projects;

/// <summary>
/// プロジェクト編集リクエストDTO
/// </summary>
public class UpdateProjectRequest
{
    /// <summary>
    /// プロジェクト名
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    /// 説明文
    /// </summary>
    public string? Description { get; set; }
}
