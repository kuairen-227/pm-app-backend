using MediatR;

namespace WebApi.Application.Commands.Projects.UpdateProject;

public class UpdateProjectCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
}
