using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate.Events;

public sealed class ProjectRoleChangedEvent : DomainEvent
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public ProjectRole.RoleType Role { get; }

    public ProjectRoleChangedEvent(
        Guid projectId, Guid userId, ProjectRole.RoleType role, IDateTimeProvider clock
    ) : base(clock)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
    }
}
