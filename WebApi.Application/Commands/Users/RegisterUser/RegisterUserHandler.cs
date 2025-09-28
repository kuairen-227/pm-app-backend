using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Commands.Users.RegisterUser;

public class RegisterUserHandler : BaseCommandHandler, IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(
            request.Name,
            request.Email,
            request.Role,
            UserContext.Id,
            Clock
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return user.Id;
    }
}
