using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Commands.Projects.DeleteProject;

[RequiresPermission(ProjectPermissions.Delete)]
public class DeleteProjectCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }

    public DeleteProjectCommand(Guid id)
    {
        ProjectId = id;
    }
}
