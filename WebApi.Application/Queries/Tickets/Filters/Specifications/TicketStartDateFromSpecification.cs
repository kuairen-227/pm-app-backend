using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketStartDateFromSpecification : ISpecification<Ticket>
{
    private readonly DateOnly _startDateFrom;
    public TicketStartDateFromSpecification(DateOnly startDateFrom) => _startDateFrom = startDateFrom;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.Schedule.StartDate != null && ticket.Schedule.StartDate.Value >= _startDateFrom;
    }
}
