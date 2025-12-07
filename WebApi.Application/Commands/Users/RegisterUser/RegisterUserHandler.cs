using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Users.RegisterUser;

public class RegisterUserHandler : BaseCommandHandler, IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHashService _passwordHashService;

    public RegisterUserHandler(
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

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(
            request.Name,
            request.Email,
            _passwordHashService.Hash(request.Password),
            request.Role,
            UserContext.Id,
            Clock
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return user.Id;
    }
}
