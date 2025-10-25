using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketDeadlineFromSpecification : ISpecification<Ticket>
{
    private readonly DateOnly _deadline;
    public TicketDeadlineFromSpecification(DateOnly deadline) => _deadline = deadline;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.Deadline != null && ticket.Deadline.Value >= _deadline;
    }
}
