using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Projects.UpdateProject;

[RequiresPermission(ProjectPermissions.Update)]
public class UpdateProjectCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public string Name { get; }
    public string? Description { get; }

    public UpdateProjectCommand(Guid id, string name, string? description)
    {
        ProjectId = id;
        Name = name;
        Description = description;
    }
}
