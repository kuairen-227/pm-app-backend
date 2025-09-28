using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Aggregates.ProjectAggregate.Events;

public sealed class ProjectRoleChangedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public ProjectRole.RoleType Role { get; }
    public DateTime OccurredAt { get; }

    public ProjectRoleChangedEvent(
        Guid projectId, Guid userId, ProjectRole.RoleType role, IDateTimeProvider clock)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
        OccurredAt = clock.Now;
    }
}
