using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Projects.LaunchProject;

[RequiresPermission(ProjectPermissions.Launch)]
public class LaunchProjectCommand : IRequest<Guid>
{
    public string Name { get; }
    public string? Description { get; }

    public LaunchProjectCommand(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
