using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Common.Security;

public interface IPermissionService
{
    void EnsurePermission(User user, Permission permission, object? context = null);
}
