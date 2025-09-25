using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Common.Authorization;

public interface IPermissionService
{
    Task EnsurePermissionAsync(
        User user,
        string permissionCode,
        Guid? projectId = null,
        CancellationToken cancellationToken = default
    );
}
