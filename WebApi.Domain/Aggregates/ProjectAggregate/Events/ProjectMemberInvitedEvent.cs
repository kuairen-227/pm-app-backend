using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate.Events;

public sealed class ProjectMemberInvitedEvent : DomainEvent
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }

    public ProjectMemberInvitedEvent(
        Guid projectId, Guid userId, IDateTimeProvider clock
    ) : base(clock)
    {
        ProjectId = projectId;
        UserId = userId;
    }
}
