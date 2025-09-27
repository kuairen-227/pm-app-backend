using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;

namespace WebApi.Application.Common;

public abstract class BaseEventHandler
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IUserContext UserContext;

    protected BaseEventHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        UnitOfWork = unitOfWork;
        UserContext = userContext;
    }
}
