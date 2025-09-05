using WebApi.Domain.Abstractions;

namespace WebApi.Application.Common;

public abstract class BaseHandler
{
    protected readonly IUserContext UserContext;
    protected readonly IDateTimeProvider Clock;

    protected BaseHandler(IUserContext userContext, IDateTimeProvider clock)
    {
        UserContext = userContext;
        Clock = clock;
    }
}
