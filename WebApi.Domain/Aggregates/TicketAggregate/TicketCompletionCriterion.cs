using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class TicketCompletionCriterion : Entity
{
    public string Criterion { get; private set; } = null!;
    public bool IsCompleted { get; private set; }

    private TicketCompletionCriterion() { } // EF Core 用

    public TicketCompletionCriterion(
        string criterion,
        Guid createdBy,
        IDateTimeProvider clock
    ) : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(criterion))
            throw new DomainException("COMPLETION_CRITERION_REQUIRED", "Completion Criterion は必須です");

        Criterion = criterion;
        IsCompleted = false;
    }

    public void EditCriterion(string criterion, Guid updatedBy, IDateTimeProvider clock)
    {
        if (string.IsNullOrWhiteSpace(criterion))
            throw new DomainException("COMPLETION_CRITERION_REQUIRED", "Completion Criterion は必須です");

        Criterion = criterion;
        UpdateAuditInfo(updatedBy, clock);
    }

    public void Complete(Guid updatedBy, IDateTimeProvider clock)
    {
        if (IsCompleted)
            throw new DomainException("ALREADY_COMPLETED", "Completion Criteria は既に完了しています");

        IsCompleted = true;
        UpdateAuditInfo(updatedBy, clock);
    }

    public void Reopen(Guid updatedBy, IDateTimeProvider clock)
    {
        if (!IsCompleted)
            throw new DomainException("NOT_COMPLETED", "Completion Criteria は完了していません");

        IsCompleted = false;
        UpdateAuditInfo(updatedBy, clock);
    }
}
