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
        return new TicketHistoryChangeValue(
            value switch
            {
                null => null,
                ITicketHistoryPrimitive primitive => primitive.ToPrimitive(),
                _ => value.ToString()!,
            }
        );
    }
}
