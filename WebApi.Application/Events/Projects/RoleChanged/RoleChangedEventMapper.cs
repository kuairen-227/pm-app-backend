using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Events.Projects.RoleChanged;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;

public sealed class RoleChangedEventMapper : IDomainEventMapper<ProjectRoleChangedEvent>
{
    public INotification? Map(ProjectRoleChangedEvent e) =>
        new RoleChangedNotification(
            e.ProjectId,
            e.UserId,
            e.Role
        );
}
