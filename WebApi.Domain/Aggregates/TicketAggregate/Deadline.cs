using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class Deadline : ValueObject
{
    public DateOnly Value { get; }

    private Deadline(DateOnly value, DateOnly today)
    {
        if (value <= today)
            throw new DomainException("DEADLINE_PAST_NOT_ALLOWED", "Deadline は過去にできません");

        Value = value;
    }

    public static Deadline Create(DateOnly value, IDateTimeProvider clock)
        => new Deadline(value, clock.Today);

    public static Deadline? CreateNullable(DateOnly? value, IDateTimeProvider clock)
    {
        if (value is null) return null;
        return new Deadline(value.Value, clock.Today);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
