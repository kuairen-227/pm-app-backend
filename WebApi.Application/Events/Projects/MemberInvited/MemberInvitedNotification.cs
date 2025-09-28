using MediatR;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Events.Projects.MemberInvited;

public sealed class MemberInvitedNotification : INotification
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }

    public MemberInvitedNotification(Guid projectId, Guid userId)
    {
        ProjectId = projectId;
        UserId = userId;
    }
}
