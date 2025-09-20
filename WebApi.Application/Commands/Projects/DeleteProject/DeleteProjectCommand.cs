using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

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
