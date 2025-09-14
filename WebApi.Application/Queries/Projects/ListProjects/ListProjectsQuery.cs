using MediatR;
using WebApi.Application.Queries.Projects.Dtos;

namespace WebApi.Application.Queries.Projects.ListProjects;

public class ListProjectsQuery : IRequest<IEnumerable<ProjectDto>>
{
}
