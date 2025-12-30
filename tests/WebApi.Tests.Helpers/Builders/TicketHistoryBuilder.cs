using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class TicketHistoryBuilder : BaseBuilder<TicketHistoryBuilder, TicketHistory>
{
    private Guid _ticketId = Guid.NewGuid();
    private Guid actorId = Guid.NewGuid();
    private DateTime _occurredAt = DateTime.UtcNow;
    private TicketHistoryAction _action = TicketHistoryAction.TicketCreated;
    private List<(TicketField field, TicketHistoryChangeValue? before, TicketHistoryChangeValue? after)> _changes = new();

    public TicketHistoryBuilder WithTicketId(Guid ticketId)
    {
        _ticketId = ticketId;
        return this;
    }

    public TicketHistoryBuilder WithActorId(Guid actorId)
    {
        this.actorId = actorId;
        return this;
    }

    public TicketHistoryBuilder WithOccurredAt(DateTime occurredAt)
    {
        _occurredAt = occurredAt;
        return this;
    }

    public TicketHistoryBuilder WithAction(TicketHistoryAction action)
    {
        _action = action;
        return this;
    }

    public TicketHistoryBuilder WithChanges(
        List<(TicketField field, TicketHistoryChangeValue? before, TicketHistoryChangeValue? after)> changes)
    {
        _changes = changes;
        return this;
    }

    public override TicketHistory Build()
    {
        var ticketHistory = new TicketHistory(
            _ticketId,
            actorId,
            _occurredAt,
            _action,
            _createdBy,
            _clock
        );

        foreach (var (field, before, after) in _changes)
        {
            ticketHistory.AddChange(field, before, after);
        }

        return ticketHistory;
    }
}
