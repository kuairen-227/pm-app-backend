using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketTitle : ValueObject, ITicketHistoryPrimitive
{
    public string Value { get; } = null!;

    private TicketTitle() { } // EF Core 用

    private TicketTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("TICKET_TITLE_REQUIRED", "TicketTitle は必須です");

        Value = value;
    }

    public static TicketTitle Create(string value) => new TicketTitle(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public object ToPrimitive() => Value;
}
