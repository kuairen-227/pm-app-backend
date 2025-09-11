using WebApi.Domain.Abstractions;
using WebApi.Domain.Common.Security;

namespace WebApi.Application.Common;

public abstract class BaseHandler
{
    protected readonly IPermissionService PermissionService;
    protected readonly IUserContext UserContext;
    protected readonly IDateTimeProvider Clock;

    protected BaseHandler(IPermissionService permissionService, IUserContext userContext, IDateTimeProvider clock)
    {
        PermissionService = permissionService;
        UserContext = userContext;
        Clock = clock;
    }
}
