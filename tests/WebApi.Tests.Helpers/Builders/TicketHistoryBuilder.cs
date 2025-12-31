using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class TicketHistoryBuilder : BaseBuilder<TicketHistoryBuilder, TicketHistory>
{
    private Guid _actorId = Guid.NewGuid();
    private DateTime _occurredAt = DateTime.UtcNow;
    private TicketHistoryAction _action = TicketHistoryAction.TicketCreated;

    public TicketHistoryBuilder WithActorId(Guid actorId)
    {
        _actorId = actorId;
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

    public override TicketHistory Build()
    {
        return new TicketHistory(
            _actorId,
            _occurredAt,
            _action,
            _createdBy,
            _clock
        );
    }
}
