using System.Linq.Expressions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Filters.Specifications;

public class TicketTitleSpecification : ISpecification<Ticket>
{
    private readonly string _title;
    public TicketTitleSpecification(string title) => _title = title;

    public Expression<Func<Ticket, bool>> ToExpression()
    {
        return ticket => ticket.Title.Value.Contains(_title);
    }
}
