using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketSchedule : ValueObject
{
    public DateOnly? StartDate { get; }
    public DateOnly? EndDate { get; }

    private TicketSchedule() { } // EF Core 用

    private TicketSchedule(DateOnly? startDate, DateOnly? endDate)
    {
        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            throw new DomainException("TICKET_SCHEDULE_INVALID", "StartDate は EndDate より前の日付である必要があります");

        StartDate = startDate;
        EndDate = endDate;
    }

    public static TicketSchedule Create(DateOnly? startDate, DateOnly? endDate)
        => new TicketSchedule(startDate, endDate);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
