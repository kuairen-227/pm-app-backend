using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class TicketStatus : ValueObject
{
    public enum StatusType { Todo, InProgress, Resolved, Done }
    public StatusType Value { get; }

    private TicketStatus(StatusType value)
    {
        Value = value;
    }

    public static TicketStatus Create(StatusType value) => new TicketStatus(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
