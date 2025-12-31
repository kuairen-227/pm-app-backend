using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketStatus : ValueObject, ITicketHistoryPrimitive
{
    public enum StatusType { Todo, InProgress, Resolved, Done }
    public StatusType Value { get; }

    private TicketStatus() { } // EF Core 用

    private TicketStatus(StatusType value)
    {
        Value = value;
    }

    public static TicketStatus Create(StatusType value) => new TicketStatus(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public object ToPrimitive() => Value;

    public static StatusType Parse(string value)
    {
        if (!Enum.TryParse<StatusType>(value, true, out var status))
            throw new DomainException("INVALID_TICKET_STATUS", $"TicketStatus '{value}' は無効です");

        return status;
    }
}
