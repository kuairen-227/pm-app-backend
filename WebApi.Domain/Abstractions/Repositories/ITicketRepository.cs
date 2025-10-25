using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Domain.Abstractions.Repositories;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> ListByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> ListByAssigneeIdAsync(Guid assigneeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> ListExpiringTicketsByAssigneeIdAsync(
        Guid assigneeId, TimeSpan? dueWithin, CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task DeleteAsync(Ticket ticket, CancellationToken cancellationToken = default);
}
