using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Commands.Projects.LaunchProject;

public class LaunchProjectHandler : BaseCommandHandler, IRequestHandler<LaunchProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;

    public LaunchProjectHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Guid> Handle(LaunchProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project(
            request.Name,
            request.Description,
            UserContext.Id,
            Clock
        );

        await _projectRepository.AddAsync(project, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return project.Id;
    }
}
