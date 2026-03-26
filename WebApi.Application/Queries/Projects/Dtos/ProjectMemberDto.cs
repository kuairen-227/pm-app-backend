namespace WebApi.Application.Queries.Projects.Dtos;

public class ProjectMemberDto
{
    public Guid UserId { get; set; }
    public string ProjectRole { get; set; } = string.Empty;
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UpdatedBy { get; init; }
    public DateTime UpdatedAt { get; init; }
}
