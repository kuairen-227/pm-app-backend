using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Projects.UpdateProject;

[RequiresPermission(ProjectPermissions.Update)]
public class UpdateProjectCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public string Name { get; }
    public string? Description { get; }

    public UpdateProjectCommand(Guid projectId, string name, string? description)
    {
        ProjectId = projectId;
        Name = name;
        Description = description;
    }
}
