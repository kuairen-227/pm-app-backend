using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Projects.RemoveMember;

[RequiresPermission(ProjectPermissions.RemoveMember)]
public class RemoveMemberCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }

    public RemoveMemberCommand(Guid projectId, Guid userId)
    {
        ProjectId = projectId;
        UserId = userId;
    }
}
