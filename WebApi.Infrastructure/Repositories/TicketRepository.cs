using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Infrastructure.Database;

namespace WebApi.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IDateTimeProvider _clock;

    public TicketRepository(AppDbContext dbContext, IDateTimeProvider clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }

    public async Task<IEnumerable<Ticket>> ListByProjectIdAsync(
        Guid projectId, ISpecification<Ticket>? specification = null, CancellationToken cancellationToken = default
    )
    {
        IQueryable<Ticket> query = _dbContext.Tickets
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId);

        if (specification != null)
        {
            query = query.Where(specification.ToExpression());
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Ticket>> ListByAssigneeIdAsync(
        Guid assigneeId, CancellationToken cancellationToken = default
    )
    {
        return await _dbContext.Tickets
            .AsNoTracking()
            .Where(t => t.AssigneeId == assigneeId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Ticket>> ListExpiringTicketsByAssigneeIdAsync(
        Guid assigneeId, TimeSpan dueWithin, CancellationToken cancellationToken = default
    )
    {
        var limit = _clock.Today.AddDays(dueWithin.Days);
        return await _dbContext.Tickets
            .AsNoTracking()
            .Where(t => t.AssigneeId == assigneeId)
            .Where(t => t.Deadline != null && t.Deadline.Value <= _clock.Today)
            .ToListAsync(cancellationToken);
    }

    public async Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets
            .Include(t => t.Comments)
            .Include(t => t.AssignmentHistories)
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);
    }

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        await _dbContext.Tickets.AddAsync(ticket, cancellationToken);
    }

    public Task DeleteAsync(Ticket ticket, CancellationToken cancellationToken = default)
    {
        _dbContext.Tickets.Remove(ticket);
        return Task.CompletedTask;
    }
}
