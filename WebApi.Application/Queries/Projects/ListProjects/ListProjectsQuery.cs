using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Application.Queries.Projects.Dtos;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Queries.Projects.ListProjects;

public class ListProjectsQuery : IRequest<IEnumerable<ProjectDto>>
{
}
