using WebApi.Application.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task PublishDomainEventsAsync(IDomainEventPublisher domainEventPublisher, CancellationToken cancellationToken = default);
}
