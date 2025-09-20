using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Projects.InviteMember;

public class InviteMemberHandler : BaseCommandHandler, IRequestHandler<InviteMemberCommand, Unit>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public InviteMemberHandler(
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, userContext, clock)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Project), request.ProjectId);
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        var role = ProjectRole.Create(request.ProjectRole);
        project.InviteMember(user.Id, role);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
