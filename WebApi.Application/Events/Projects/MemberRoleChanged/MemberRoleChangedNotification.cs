using MediatR;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Events.Projects.MemberRoleChanged;

public sealed class MemberRoleChangedNotification : INotification
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public ProjectRole.RoleType Role { get; }

    public MemberRoleChangedNotification(Guid projectId, Guid userId, ProjectRole.RoleType role)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
    }
}
