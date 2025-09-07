using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetAllAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task UpdateAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task DeleteAsync(Ticket ticket, CancellationToken cancellationToken = default);
}
