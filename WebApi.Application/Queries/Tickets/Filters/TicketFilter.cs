using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Application.Queries.Tickets.Filters.Specifications;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets;

public class TicketFilter
{
    public string? Title { get; init; }
    public Guid? AssigneeId { get; init; }
    public string? Status { get; init; }
    public DateOnly? StartDateFrom { get; init; }
    public DateOnly? StartDateTo { get; init; }
    public DateOnly? EndDateFrom { get; init; }
    public DateOnly? EndDateTo { get; init; }

    public ISpecification<Ticket>? ToSpecification()
    {
        ISpecification<Ticket>? spec = null;

        if (!string.IsNullOrWhiteSpace(Title))
            spec = new TicketTitleSpecification(Title);

        if (AssigneeId.HasValue)
            spec = spec == null
                ? new TicketAssigneeSpecification(AssigneeId.Value)
                : spec.And(new TicketAssigneeSpecification(AssigneeId.Value));

        if (!string.IsNullOrWhiteSpace(Status))
            spec = spec == null
                ? new TicketStatusSpecification(Status)
                : spec.And(new TicketStatusSpecification(Status));

        if (StartDateFrom.HasValue)
            spec = spec == null
                ? new TicketStartDateFromSpecification(StartDateFrom.Value)
                : spec.And(new TicketStartDateFromSpecification(StartDateFrom.Value));

        if (StartDateTo.HasValue)
            spec = spec == null
                ? new TicketStartDateToSpecification(StartDateTo.Value)
                : spec.And(new TicketStartDateToSpecification(StartDateTo.Value));

        if (EndDateFrom.HasValue)
            spec = spec == null
                ? new TicketEndDateFromSpecification(EndDateFrom.Value)
                : spec.And(new TicketEndDateFromSpecification(EndDateFrom.Value));

        if (EndDateTo.HasValue)
            spec = spec == null
                ? new TicketEndDateToSpecification(EndDateTo.Value)
                : spec.And(new TicketEndDateToSpecification(EndDateTo.Value));

        return spec;
    }
}
