using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Application.Common.Security.Permissions;

namespace WebApi.Application.Commands.Projects.UpdateProject;

[RequiresPermission(ProjectPermissions.Update)]
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
