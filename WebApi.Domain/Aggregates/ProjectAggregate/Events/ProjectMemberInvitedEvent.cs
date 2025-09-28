using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Aggregates.ProjectAggregate.Events;

public sealed class ProjectMemberInvitedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public DateTime OccurredAt { get; }

    public ProjectMemberInvitedEvent(
        Guid projectId, Guid userId, IDateTimeProvider clock)
    {
        ProjectId = projectId;
        UserId = userId;
        OccurredAt = clock.Now;
    }
}
