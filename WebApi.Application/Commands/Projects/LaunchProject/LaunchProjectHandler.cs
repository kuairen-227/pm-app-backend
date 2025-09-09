using MediatR;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Commands.Projects.CreateProject;

public class LaunchProjectHandler : BaseHandler, IRequestHandler<LaunchProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;

    public LaunchProjectHandler(IProjectRepository projectRepository, IUserContext userContext, IDateTimeProvider clock)
        : base(userContext, clock)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Guid> Handle(LaunchProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(
            request.Name,
            request.Description,
            request.OwnerId,
            UserContext.Id,
            Clock
        );
        await _projectRepository.AddAsync(project, cancellationToken);

        return project.Id;
    }
}
