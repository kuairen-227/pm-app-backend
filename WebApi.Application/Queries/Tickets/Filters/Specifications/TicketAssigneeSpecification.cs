using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketAssigneeSpecification : ISpecification<Ticket>
{
    private readonly Guid _assigneeId;
    public TicketAssigneeSpecification(Guid assigneeId) => _assigneeId = assigneeId;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.AssigneeId == _assigneeId;
    }
}
