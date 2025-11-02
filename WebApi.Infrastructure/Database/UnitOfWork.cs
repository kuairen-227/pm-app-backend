using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;

namespace WebApi.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(
        IDomainEventPublisher domainEventPublisher,
        CancellationToken cancellationToken = default
    )
    {
        var domainEvents = _dbContext.ChangeTracker
            .Entries<IHasDomainEvents>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
        }

        foreach (var entity in _dbContext.ChangeTracker.Entries<IHasDomainEvents>())
        {
            entity.Entity.ClearDomainEvents();
        }
    }
}
