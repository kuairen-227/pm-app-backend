using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketHistoryChange : ValueObject
{
    public TicketField Field { get; }
    public TicketHistoryChangeValue? Before { get; }
    public TicketHistoryChangeValue? After { get; }

    private TicketHistoryChange(
        TicketField field,
        TicketHistoryChangeValue? before,
        TicketHistoryChangeValue? after)
    {
        if (Equals(before?.Value, after?.Value))
        {
            throw new DomainException(
                "NO_CHANGE_IN_HISTORY",
                "Before と After の値が同じ場合、 TicketHistoryChangeValue を作成できません"
            );
        }

        Field = field;
        Before = before;
        After = after;
    }

    public static TicketHistoryChange Create(
        TicketField field,
        TicketHistoryChangeValue? before,
        TicketHistoryChangeValue? after)
    {
        return new TicketHistoryChange(field, before, after);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Field;
        yield return Before;
        yield return After;
    }
}
