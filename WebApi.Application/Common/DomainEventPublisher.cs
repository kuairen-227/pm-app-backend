using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Events.Projects.MemberInvited;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;

namespace WebApi.Application.Common;

public sealed class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;
    private readonly IEnumerable<IDomainEventMapper<IDomainEvent>> _mappers;

    public DomainEventPublisher(
        IMediator mediator, IEnumerable<IDomainEventMapper<IDomainEvent>> mappers)
    {
        _mediator = mediator;
        _mappers = mappers;
    }

    public async Task PublishAsync<TDomainEvent>(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
        where TDomainEvent : IDomainEvent
    {
        foreach (var mapper in _mappers)
        {
            var notification = mapper.Map(domainEvent);
            if (notification is not null)
            {
                await _mediator.Publish(notification, cancellationToken);
            }
        }
    }
}
