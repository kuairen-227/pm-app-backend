namespace WebApi.Application.Queries.Projects.Dtos;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
