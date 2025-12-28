using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketEndDateToSpecification : ISpecification<Ticket>
{
    private readonly DateOnly _endDateTo;
    public TicketEndDateToSpecification(DateOnly endDateTo) => _endDateTo = endDateTo;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.Schedule.EndDate != null && ticket.Schedule.EndDate.Value <= _endDateTo;
    }
}
