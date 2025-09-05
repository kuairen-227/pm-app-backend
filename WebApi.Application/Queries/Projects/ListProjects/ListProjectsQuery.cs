using MediatR;

namespace WebApi.Application.Queries.Projects.ListProjects;

public class ListProjectsQuery : IRequest<IEnumerable<ProjectDto>>
{
}
