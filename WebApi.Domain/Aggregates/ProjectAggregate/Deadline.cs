using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Deadline : ValueObject
{
    public DateTime Value { get; }

    private Deadline(DateTime value)
    {
        if (value < DateTime.UtcNow.Date)
            throw new ArgumentException("Deadline は過去にできません");

        Value = value;
    }

    public static Deadline Create(DateTime value) => new Deadline(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
