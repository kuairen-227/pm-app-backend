using MediatR;

namespace WebApi.Application.Commands.Projects.CreateProject;

public class CreateProjectCommand : IRequest<Guid>
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid OwnerId { get; set; }
}
