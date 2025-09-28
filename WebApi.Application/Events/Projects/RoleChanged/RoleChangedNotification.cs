using MediatR;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Events.Projects.RoleChanged;

public sealed class RoleChangedNotification : INotification
{
    public Guid ProjectId { get; }
    public Guid UserId { get; }
    public ProjectRole.RoleType Role { get; }

    public RoleChangedNotification(Guid projectId, Guid userId, ProjectRole.RoleType role)
    {
        ProjectId = projectId;
        UserId = userId;
        Role = role;
    }
}
