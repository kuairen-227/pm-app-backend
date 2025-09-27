using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;

namespace WebApi.Application.Common;

public abstract class BaseCommandHandler
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IDomainEventPublisher DomainEventPublisher;
    protected readonly IUserContext UserContext;
    protected readonly IDateTimeProvider Clock;

    protected BaseCommandHandler(
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock)
    {
        UnitOfWork = unitOfWork;
        DomainEventPublisher = domainEventPublisher;
        UserContext = userContext;
        Clock = clock;
    }
}
