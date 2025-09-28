using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;

namespace WebApi.Application.Events.Projects.MemberRoleChanged;

public sealed class MemberRoleChangedEventMapper : IDomainEventMapper<ProjectRoleChangedEvent>
{
    public INotification? Map(ProjectRoleChangedEvent e) =>
        new MemberRoleChangedNotification(
            e.ProjectId,
            e.UserId,
            e.Role
        );
}
