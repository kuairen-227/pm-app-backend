using MediatR;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Events.Projects.MemberInvited;

public sealed class MemberInvitedNotification : INotification
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public ProjectRole.RoleType Role { get; }

    public MemberInvitedNotification(Guid projectId, Guid userId, ProjectRole.RoleType role)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
    }
}
