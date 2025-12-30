using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketDescription : ValueObject, ITicketHistoryPrimitive
{
    public string Value { get; } = string.Empty;

    private TicketDescription() { } // EF Core 用

    private TicketDescription(string value)
    {
        Value = value;
    }

    public static TicketDescription Create(string value) => new TicketDescription(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public object ToPrimitive() => Value;
}
