using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Common.Security;

public class PermissionService : IPermissionService
{
    public void EnsurePermission(User user, string permissionCode, object? context = null)
    {
        if (!RolePermissions.Map.TryGetValue(user.Role.Value, out var permissions)
            || !permissions.Contains(permissionCode))
        {
            throw new DomainException("FORBIDDEN", $"{permissionCode}: 権限がありません");
        }
    }
}
