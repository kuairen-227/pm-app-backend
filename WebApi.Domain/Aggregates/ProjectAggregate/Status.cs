using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Status : ValueObject
{
    public enum StatusType { Todo, InProgress, Resolved, Done }
    public StatusType Value { get; }

    private Status(StatusType value)
    {
        Value = value;
    }

    public static Status Create(StatusType value) => new Status(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
