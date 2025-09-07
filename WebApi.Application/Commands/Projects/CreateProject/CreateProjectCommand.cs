using MediatR;

namespace WebApi.Application.Commands.Projects.CreateProject;

public class CreateProjectCommand : IRequest<Guid>
{
    public string Name { get; }
    public string? Description { get; }
    public Guid OwnerId { get; }

    public CreateProjectCommand(string name, string? description, Guid ownerId)
    {
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }
}
