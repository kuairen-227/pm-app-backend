using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common.Authorization;

namespace WebApi.Application.Common.Authorization;

public class PermissionService : IPermissionService
{
    private readonly IProjectRepository _projectRepository;

    public PermissionService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task EnsurePermissionAsync(
        User user,
        string permissionCode,
        Guid? projectId = null,
        CancellationToken cancellationToken = default)
    {
        // SystemRole による認可
        if (SystemRolePermissions.Map.TryGetValue(user.Role.Value, out var systemPermissions)
            && systemPermissions.Contains(permissionCode))
        {
            return;
        }

        // ProjectRole による認可
        if (projectId.HasValue)
        {
            var project = await _projectRepository.GetByIdAsync(projectId.Value, cancellationToken)
                ?? throw new NotFoundException(nameof(Project), projectId.Value);

            var membership = project.Members.FirstOrDefault(m => m.UserId == user.Id)
                ?? throw new AuthorizationException("FORBIDDEN", "プロジェクトに所属していません");

            if (ProjectRolePermissions.Map.TryGetValue(membership.Role.Value, out var projectPermissions)
                && projectPermissions.Contains(permissionCode))
            {
                return;
            }
        }

        throw new AuthorizationException("FORBIDDEN", $"{permissionCode}: 権限がありません");
    }
}
