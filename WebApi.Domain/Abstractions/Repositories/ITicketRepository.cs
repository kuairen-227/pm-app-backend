using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Abstractions.Repositories;

public interface ITicketRepository
{
    Task<PagedResult<Ticket>> ListByProjectIdAsync(
        Guid projectId,
        ISpecification<Ticket>? specification = null,
        int skip = 0,
        int take = 20,
        string? sortBy = null,
        SortOrder? sortOrder = SortOrder.Desc,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> ListByAssigneeIdAsync(Guid assigneeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Ticket>> ListExpiringTicketsByAssigneeIdAsync(
        Guid assigneeId, TimeSpan dueWithin, CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task DeleteAsync(Ticket ticket, CancellationToken cancellationToken = default);
}
