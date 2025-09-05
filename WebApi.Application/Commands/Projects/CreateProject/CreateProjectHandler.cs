using MediatR;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Commands.Projects.CreateProject;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _clock;

    public CreateProjectHandler(IProjectRepository projectRepository, IUserContext userContext, IDateTimeProvider clock)
    {
        _projectRepository = projectRepository;
        _userContext = userContext;
        _clock = clock;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(
            request.Name,
            request.Description,
            request.OwnerId,
            _userContext.Id,
            _clock
        );
        await _projectRepository.AddAsync(project, cancellationToken);

        return project.Id;
    }
}
