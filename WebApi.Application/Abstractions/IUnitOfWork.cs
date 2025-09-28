using WebApi.Application.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync(
        IDomainEventPublisher domainEventPublisher, CancellationToken cancellationToken = default);
}
