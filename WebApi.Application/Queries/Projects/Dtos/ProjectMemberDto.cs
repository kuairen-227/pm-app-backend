namespace WebApi.Application.Queries.Projects.Dtos;

public class ProjectMemberDto
{
    public Guid UserId { get; set; }
    public string ProjectRole { get; set; } = string.Empty;
}
