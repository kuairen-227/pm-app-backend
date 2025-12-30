using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketHistoryChangeValue
{
    public string Kind { get; }
    public object? Value { get; }

    private TicketHistoryChangeValue(string kind, object? value)
    {
        Kind = kind;
        Value = value;
    }

    public static TicketHistoryChangeValue? From(object? value)
    {
        if (value is null) return null;

        return new TicketHistoryChangeValue(
            value.GetType().Name,
            value switch
            {
                ITicketHistoryPrimitive primitive => primitive.ToPrimitive(),
                _ => value.ToString()!,
            }
        );
    }
}
