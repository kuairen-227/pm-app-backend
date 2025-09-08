using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class Deadline : ValueObject
{
    public DateTimeOffset Value { get; }

    private Deadline(DateTimeOffset value, DateTimeOffset now)
    {
        if (value <= now)
            throw new DomainException("DEADLINE_PAST_NOT_ALLOWED", "Deadline は過去にできません");

        Value = value;
    }

    public static Deadline Create(DateTimeOffset value, IDateTimeProvider clock)
        => new Deadline(value, clock.Now);

    public static Deadline? CreateNullable(DateTimeOffset? value, IDateTimeProvider clock)
    {
        if (value is null) return null;
        return new Deadline(value.Value, clock.Now);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
