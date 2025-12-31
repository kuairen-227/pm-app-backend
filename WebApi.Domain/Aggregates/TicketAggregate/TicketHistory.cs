using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketHistory : Entity
{
    public Guid ActorId { get; private set; }
    public DateTime OccurredAt { get; private set; }
    public TicketHistoryAction Action { get; private set; }

    private readonly List<TicketHistoryChange> _changes = new();
    public IReadOnlyCollection<TicketHistoryChange> Changes => _changes.AsReadOnly();

    private TicketHistory() { } // EF Core 用

    public TicketHistory(
        Guid actorId,
        DateTime occurredAt,
        TicketHistoryAction action,
        Guid createdBy,
        IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (actorId == Guid.Empty)
            throw new DomainException("ACTOR_ID_REQUIRED", "ActorId は必須です");

        ActorId = actorId;
        OccurredAt = occurredAt;
        Action = action;
    }

    public void AddChange(
        TicketField field,
        TicketHistoryChangeValue? before,
        TicketHistoryChangeValue? after)
    {
        var change = TicketHistoryChange.Create(field, before, after);
        _changes.Add(change);
    }
}
