using MediatR;

namespace WebApi.Application.Commands.Projects.LaunchProject;

public class LaunchProjectCommand : IRequest<Guid>
{
    public string Name { get; }
    public string? Description { get; }
    public Guid OwnerId { get; }

    public LaunchProjectCommand(string name, string? description, Guid ownerId)
    {
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }
}
