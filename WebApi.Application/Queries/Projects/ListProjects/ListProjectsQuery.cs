using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Queries.Projects.Dtos;

namespace WebApi.Application.Queries.Projects.ListProjects;

[RequiresPermission(ProjectPermissions.View)]
public class ListProjectsQuery : IRequest<IReadOnlyList<ProjectDto>>
{
}
