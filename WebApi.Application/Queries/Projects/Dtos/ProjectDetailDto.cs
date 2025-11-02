namespace WebApi.Application.Queries.Projects.Dtos;

public class ProjectDetailDto : ProjectBaseDto
{
    public IReadOnlyList<ProjectMemberDto> Members { get; set; } = Array.Empty<ProjectMemberDto>();
}
