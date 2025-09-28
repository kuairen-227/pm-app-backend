using MediatR;
using WebApi.Domain.Abstractions;

namespace WebApi.Application.Abstractions;

public interface IDomainEventMapper<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    INotification? Map(TDomainEvent domainEvent);
}
