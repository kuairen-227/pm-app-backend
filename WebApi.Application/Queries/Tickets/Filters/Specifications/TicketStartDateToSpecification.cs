using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketStartDateToSpecification : ISpecification<Ticket>
{
    private readonly DateOnly _startDateTo;
    public TicketStartDateToSpecification(DateOnly startDateTo) => _startDateTo = startDateTo;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.Schedule.StartDate != null && ticket.Schedule.StartDate.Value <= _startDateTo;
    }
}
