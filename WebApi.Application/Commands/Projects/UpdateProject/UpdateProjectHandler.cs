using MediatR;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Projects.UpdateProject;

public class UpdateProjectHandler : BaseHandler, IRequestHandler<UpdateProjectCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;

    public UpdateProjectHandler(IProjectRepository projectRepository, IUserContext userContext, IDateTimeProvider clock)
        : base(userContext, clock)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("PROJECT_NOT_FOUND", "Project が見つかりません");

        project.Rename(request.Name, UserContext.Id, Clock);
        project.ChangeDescription(request.Description, UserContext.Id, Clock);
        project.ChangeOwner(request.OwnerId, UserContext.Id, Clock);

        await _projectRepository.UpdateAsync(project, cancellationToken);
        return Unit.Value;
    }
}
