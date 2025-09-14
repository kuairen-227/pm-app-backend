using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Projects.DeleteProject;

public class DeleteProjectHandler : BaseCommandHandler, IRequestHandler<DeleteProjectCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectHandler(
        IProjectRepository projectRepository, IUnitOfWork unitOfWork, IUserContext userContext, IDateTimeProvider clock)
        : base(unitOfWork, userContext, clock)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("PROJECT_NOT_FOUND", "Project が見つかりません");
        project.EnsureDeletable(UserContext.Id);

        await _projectRepository.DeleteAsync(project, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
