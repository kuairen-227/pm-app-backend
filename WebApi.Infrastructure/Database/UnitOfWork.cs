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
        // 1. 永続化
        await _dbContext.SaveChangesAsync(cancellationToken);

        // 2. ドメインイベント発行
        var entries = _dbContext.ChangeTracker.Entries<IHasDomainEvents>().ToList();
        if (entries.Count == 0) return;

        var domainEvents = entries
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();
        if (domainEvents.Count == 0) return;

        foreach (var domainEvent in domainEvents)
        {
            await domainEventPublisher.PublishAsync(domainEvent, cancellationToken);
        }

        foreach (var entry in entries)
        {
            entry.Entity.ClearDomainEvents();
        }
    }
}
