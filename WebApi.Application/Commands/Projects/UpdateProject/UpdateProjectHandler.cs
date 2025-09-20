using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Projects.UpdateProject;

public class UpdateProjectHandler : BaseCommandHandler, IRequestHandler<UpdateProjectCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, userContext, clock)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException("PROJECT_NOT_FOUND", "Project が見つかりません");

        project.Rename(request.Name, UserContext.Id, Clock);
        project.ChangeDescription(request.Description, UserContext.Id, Clock);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
