using MediatR;

namespace WebApi.Application.Commands.Projects.UpdateProject;

public class UpdateProjectCommand : IRequest<Unit>
{
    public Guid Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public Guid OwnerId { get; }

    public UpdateProjectCommand(Guid id, string name, string? description, Guid ownerId)
    {
        Id = id;
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }
}
