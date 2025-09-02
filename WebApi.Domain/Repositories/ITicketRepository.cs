using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetAllAsync(Guid projectId);
    Task<Ticket?> GetByIdAsync(Guid ticketId);
    Task AddAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
    Task DeleteAsync(Ticket ticket);
}
