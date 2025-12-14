using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Users.UpdateUser;

public class UpdateUserHandler : BaseCommandHandler, IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;

    public UpdateUserHandler(
        IUserRepository userRepository,
        IPasswordHashService passwordHashService,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _userRepository = userRepository;
        _passwordHashService = passwordHashService;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.UserId);

        if (request.Name is not null)
            user.ChangeName(request.Name);
        if (request.Email is not null)
            user.ChangeEmail(request.Email);
        if (request.Password is not null)
            user.ChangePassword(_passwordHashService.Hash(request.Password));
        if (request.Role is not null)
            user.ChangeUserRole(request.Role.Value);

        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);
        return Unit.Value;
    }
}
