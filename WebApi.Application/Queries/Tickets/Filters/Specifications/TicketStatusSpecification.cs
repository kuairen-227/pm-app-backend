using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketStatusSpecification : ISpecification<Ticket>
{
    private readonly string _status;
    public TicketStatusSpecification(string status) => _status = status;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.Status.Value.ToString() == _status;
    }
}
