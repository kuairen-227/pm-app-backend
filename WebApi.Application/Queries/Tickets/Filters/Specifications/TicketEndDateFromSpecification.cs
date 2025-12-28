using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketEndDateFromSpecification : ISpecification<Ticket>
{
    private readonly DateOnly _endDateFrom;
    public TicketEndDateFromSpecification(DateOnly endDateFrom) => _endDateFrom = endDateFrom;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.Schedule.EndDate != null && ticket.Schedule.EndDate.Value >= _endDateFrom;
    }
}
