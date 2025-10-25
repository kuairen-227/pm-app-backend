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
    public DateOnly? DeadlineFrom { get; init; }
    public DateOnly? DeadlineTo { get; init; }

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

        if (DeadlineFrom.HasValue)
            spec = spec == null
                ? new TicketDeadlineFromSpecification(DeadlineFrom.Value)
                : spec.And(new TicketDeadlineFromSpecification(DeadlineFrom.Value));

        if (DeadlineTo.HasValue)
            spec = spec == null
                ? new TicketDeadlineToSpecification(DeadlineTo.Value)
                : spec.And(new TicketDeadlineToSpecification(DeadlineTo.Value));

        return spec;
    }
}
