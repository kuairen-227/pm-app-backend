using MediatR;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Projects.ListProjects;

public class ListProjectsHandler : IRequestHandler<ListProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;

    public ListProjectsHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync(cancellationToken);

        return projects.Select(project => new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            CreatedBy = project.CreatedBy,
            CreatedAt = project.CreatedAt,
            UpdatedBy = project.UpdatedBy,
            UpdatedAt = project.UpdatedAt
        });
    }
}
