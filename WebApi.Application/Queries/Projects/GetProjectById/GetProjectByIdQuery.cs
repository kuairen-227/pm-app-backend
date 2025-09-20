using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Queries.Projects.Dtos;
using WebApi.Domain.Common.Authorization.Permissions;

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
