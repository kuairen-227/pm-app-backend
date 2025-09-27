using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Events.Projects.InviteMember;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;

namespace WebApi.Application.Common;

public sealed class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public DomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
        where TDomainEvent : IDomainEvent
    {
        switch (domainEvent)
        {
            case ProjectMemberInvitedEvent e:
                var notification = new ProjectMemberInvitedNotification(
                    e.ProjectId, e.UserId, e.Role
                );
                await _mediator.Publish(notification, cancellationToken);
                break;
        }
    }
}
