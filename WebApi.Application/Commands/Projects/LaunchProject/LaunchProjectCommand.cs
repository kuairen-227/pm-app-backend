using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Application.Common.Security.Permissions;

namespace WebApi.Application.Commands.Projects.LaunchProject;

[RequiresPermission(ProjectPermissions.Launch)]
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
