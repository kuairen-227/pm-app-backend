using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Projects.ChangeMemberRole;

[RequiresPermission(ProjectPermissions.ChangeMemberRole)]
public class ChangeMemberRoleCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public ProjectRole.RoleType ProjectRole { get; }

    public ChangeMemberRoleCommand(Guid projectId, Guid userId, ProjectRole.RoleType projectRole)
    {
        ProjectId = projectId;
        UserId = userId;
        ProjectRole = projectRole;
    }
}
