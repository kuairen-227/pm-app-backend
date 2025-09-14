using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;

namespace WebApi.Application.Common;

public abstract class BaseCommandHandler
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IUserContext UserContext;
    protected readonly IDateTimeProvider Clock;

    protected BaseCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext, IDateTimeProvider clock)
    {
        UnitOfWork = unitOfWork;
        UserContext = userContext;
        Clock = clock;
    }
}
