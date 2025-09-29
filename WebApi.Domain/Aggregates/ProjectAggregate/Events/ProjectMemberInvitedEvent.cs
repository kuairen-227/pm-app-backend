using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate.Events;

public sealed class ProjectMemberInvitedEvent : DomainEvent
{
    public Guid ProjectId { get; }
    public string ProjectName { get; }
    public Guid UserId { get; }

    public ProjectMemberInvitedEvent(
        Guid projectId, string projectName, Guid userId, IDateTimeProvider clock
    ) : base(clock)
    {
        ProjectId = projectId;
        ProjectName = projectName;
        UserId = userId;
    }
}
