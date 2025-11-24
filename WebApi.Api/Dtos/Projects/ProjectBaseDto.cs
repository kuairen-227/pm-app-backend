using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Dtos.Projects;

/// <summary>
/// Project ベースDTO
/// </summary>
public abstract class ProjectBaseDto
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
