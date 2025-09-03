using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class Deadline : ValueObject
{
    public DateTimeOffset Value { get; }

    private Deadline(DateTimeOffset value)
    {
        if (value < DateTimeOffset.UtcNow)
            throw new DomainException("DEADLINE_PAST_NOT_ALLOWED", "Deadline は過去にできません");

        Value = value;
    }

    public static Deadline Create(DateTimeOffset value) => new Deadline(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
