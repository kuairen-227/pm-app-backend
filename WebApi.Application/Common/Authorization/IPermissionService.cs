using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Common.Authorization;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(
        User user,
        string permissionCode,
        Guid? projectId = null,
        CancellationToken cancellationToken = default
    );
}
