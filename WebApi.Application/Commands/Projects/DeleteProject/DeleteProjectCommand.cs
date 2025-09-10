using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Application.Common.Security.Permissions;

namespace WebApi.Application.Commands.Projects.DeleteProject;

[RequiresPermission(ProjectPermissions.Delete)]
public class DeleteProjectCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public DeleteProjectCommand(Guid id)
    {
        Id = id;
    }
}
