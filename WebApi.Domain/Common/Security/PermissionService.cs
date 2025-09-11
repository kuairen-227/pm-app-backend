using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Common.Security;

public class PermissionService : IPermissionService
{
    public void EnsurePermission(User user, Permission permission, object? context = null)
    {
        if (!RolePermissions.Map.TryGetValue(user.Role.Value, out var permissions)
            || !permissions.Contains(permission))
        {
            throw new DomainException("FORBIDDEN", $"{permission.Code}: 権限がありません");
        }
    }
}
