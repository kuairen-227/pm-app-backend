using WebApi.Domain.Abstractions;
using WebApi.Domain.Common.Security;

namespace WebApi.Application.Common;

public abstract class BaseHandler
{
    protected readonly IAuthorizationService AuthService;
    protected readonly IUserContext UserContext;
    protected readonly IDateTimeProvider Clock;

    protected BaseHandler(IAuthorizationService authService, IUserContext userContext, IDateTimeProvider clock)
    {
        AuthService = authService;
        UserContext = userContext;
        Clock = clock;
    }
}
