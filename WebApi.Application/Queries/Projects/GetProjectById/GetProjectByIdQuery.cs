using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Queries.Projects.Dtos;

namespace WebApi.Application.Queries.Projects.GetProjectById;

[RequiresPermission(ProjectPermissions.View)]
public class GetProjectByIdQuery : IRequest<ProjectDto?>, IProjectScopedRequest
{
    public Guid ProjectId { get; }

    public GetProjectByIdQuery(Guid projectId)
    {
        ProjectId = projectId;
    }
}
