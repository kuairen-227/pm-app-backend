using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Commands.Projects.UpdateProject;

[RequiresPermission(ProjectPermissions.Update)]
public class UpdateProjectCommand : IRequest<Unit>
{
    public Guid Id { get; }
    public string Name { get; }
    public string? Description { get; }

    public UpdateProjectCommand(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
