using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketHistoryChangeValue
{
    public object? Value { get; }

    private TicketHistoryChangeValue(object? value)
    {
        Value = value;
    }

    public static TicketHistoryChangeValue? From(object? value)
    {
        if (value is null) return null;

        return new TicketHistoryChangeValue(
            value switch
            {
                ITicketHistoryPrimitive primitive => primitive.ToPrimitive(),
                _ => value.ToString()!,
            }
        );
    }
}
