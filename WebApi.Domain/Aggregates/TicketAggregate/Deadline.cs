using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class Deadline : ValueObject
{
    public DateTime Value { get; }

    private Deadline(DateTime value, DateTime now)
    {
        if (value <= DateTime.FromDateTime(now))
            throw new DomainException("DEADLINE_PAST_NOT_ALLOWED", "Deadline は過去にできません");

        Value = value;
    }

    public static Deadline Create(DateTime value, IDateTimeProvider clock)
        => new Deadline(value, clock.Now);

    public static Deadline? CreateNullable(DateTime? value, IDateTimeProvider clock)
    {
        if (value is null) return null;
        return new Deadline(value.Value, clock.Now);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
