using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Users.ChangeUserRole;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Users.ChangeUserRole;

public class ChangeUserRoleHandler : BaseCommandHandler, IRequestHandler<ChangeUserRoleCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public ChangeUserRoleHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        user.ChangeUserRole(user.Id, request.SystemRole);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return Unit.Value;
    }
}
