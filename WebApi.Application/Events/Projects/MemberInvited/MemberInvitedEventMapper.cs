using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Events.Projects.MemberInvited;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;

public sealed class MemberInvitedEventMapper : IDomainEventMapper<ProjectMemberInvitedEvent>
{
    public INotification? Map(ProjectMemberInvitedEvent e) =>
        new MemberInvitedNotification(
            e.ProjectId,
            e.UserId,
            e.Role
        );
}
