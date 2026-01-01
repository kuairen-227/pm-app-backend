using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate.Events;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class Ticket : Entity
{
    public Guid ProjectId { get; private set; }
    public TicketTitle Title { get; private set; } = null!;
    public TicketDescription Description { get; private set; } = null!;
    public Guid? AssigneeId { get; private set; }
    public TicketSchedule Schedule { get; private set; } = null!;
    public TicketStatus Status { get; private set; } = null!;

    public readonly List<TicketCompletionCriterion> _completionCriteria = new();
    public IReadOnlyList<TicketCompletionCriterion> CompletionCriteria => _completionCriteria.AsReadOnly();

    private readonly List<TicketComment> _comments = new();
    public IReadOnlyList<TicketComment> Comments => _comments.AsReadOnly();

    private readonly List<TicketHistory> _histories = new();
    public IReadOnlyList<TicketHistory> Histories => _histories.AsReadOnly();
    private readonly List<TicketHistoryChange> _pendingChanges = new();

    private Ticket() { } // EF Core 用

    public Ticket(
        Guid projectId,
        string title,
        string description,
        Guid? assigneeId,
        DateOnly? startDate,
        DateOnly? endDate,
        List<string>? completionCriteria,
        Guid createdBy,
        IDateTimeProvider clock
    ) : base(createdBy, clock)
    {
        if (projectId == Guid.Empty)
            throw new DomainException("PROJECT_ID_REQUIRED", "Project ID は必須です");

        ProjectId = projectId;
        Title = TicketTitle.Create(title);
        Description = TicketDescription.Create(description);
        AssigneeId = assigneeId;
        Schedule = TicketSchedule.Create(startDate, endDate);
        Status = TicketStatus.Create(TicketStatus.StatusType.Todo);

        if (completionCriteria != null)
        {
            foreach (var criterion in completionCriteria)
            {
                var completionCriterion = new TicketCompletionCriterion(
                    criterion,
                    createdBy,
                    clock
                );
                _completionCriteria.Add(completionCriterion);
            }
        }
    }

    public void ChangeTitle(string title, Guid updatedBy)
    {
        var newTitle = TicketTitle.Create(title);
        ChangeWithHistory(
            TicketField.Title,
            Title,
            newTitle,
            () => Title = newTitle
        );
        UpdateAuditInfo(updatedBy);
    }

    public void ChangeDescription(string description, Guid updatedBy)
    {
        var newDescription = TicketDescription.Create(description);
        ChangeWithHistory(
            TicketField.Description,
            Description,
            newDescription,
            () => Description = newDescription
        );
        UpdateAuditInfo(updatedBy);
    }

    public void Assign(Guid assigneeId, string assigneeName, IReadOnlyCollection<Guid> notificationRecipientIds, Guid updatedBy)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");
        if (AssigneeId == assigneeId)
            throw new DomainException("ALREADY_ASSIGNED_SAME_USER", "既に同じユーザーに割り当てられています");

        ChangeWithHistory(
            TicketField.Assignee,
            AssigneeId,
            assigneeId,
            () => AssigneeId = assigneeId
        );

        UpdateAuditInfo(updatedBy);
        AddDomainEvent(new TicketMemberAssignedEvent(
            notificationRecipientIds, Id, Title, assigneeId, assigneeName, ProjectId, _clock
        ));
    }

    public void Unassign(Guid updatedBy)
    {
        if (AssigneeId is null)
            throw new DomainException("NOT_ASSIGNED", "現在割り当てられていません");

        ChangeWithHistory(
            TicketField.Assignee,
            AssigneeId,
            null,
            () => AssigneeId = null
        );

        UpdateAuditInfo(updatedBy);
    }

    public void ChangeSchedule(DateOnly? newStartDate, DateOnly? newEndDate, Guid updatedBy)
    {
        if (Schedule.StartDate != newStartDate)
        {
            ChangeWithHistory(
                TicketField.StartDate,
                Schedule.StartDate,
                newStartDate,
                () => { }
            );
        }

        if (Schedule.EndDate != newEndDate)
        {
            ChangeWithHistory(
                TicketField.EndDate,
                Schedule.EndDate,
                newEndDate,
                () => { }
            );
        }

        Schedule = TicketSchedule.Create(newStartDate, newEndDate);
        UpdateAuditInfo(updatedBy);
    }

    public void ChangeStatus(TicketStatus.StatusType status, Guid updatedBy)
    {
        var newStatus = TicketStatus.Create(status);
        ChangeWithHistory(
            TicketField.Status,
            Status,
            newStatus,
            () => Status = newStatus
        );
        UpdateAuditInfo(updatedBy);
    }

    public void AddCompletionCriterion(string criterion, Guid createdBy)
    {
        var newCriterion = new TicketCompletionCriterion(
            criterion,
            createdBy,
            _clock
        );
        ChangeWithHistory(
            TicketField.CompletionCriterion,
            null,
            criterion,
            () => _completionCriteria.Add(newCriterion)
        );
        RecalculateStatus(createdBy);
    }

    public void EditCompletionCriterion(Guid criterionId, string newCriterion, Guid updatedBy)
    {
        var criterion = _completionCriteria.FirstOrDefault(c => c.Id == criterionId)
            ?? throw new DomainException("TICKET_COMPLETION_CRITERION_NOT_FOUND", "Ticket Completion Criterion が見つかりません");

        var before = criterion.Criterion;

        ChangeWithHistory(
            TicketField.CompletionCriterion,
            before,
            newCriterion,
            () => criterion.EditCriterion(newCriterion, updatedBy)
        );
    }

    public void RemoveCompletionCriterion(Guid criterionId, Guid deletedBy)
    {
        var criterion = _completionCriteria.FirstOrDefault(c => c.Id == criterionId)
            ?? throw new DomainException("TICKET_COMPLETION_CRITERION_NOT_FOUND", "Ticket Completion Criterion が見つかりません");
        ChangeWithHistory(
            TicketField.CompletionCriterion,
            criterion.Criterion,
            null,
            () => _completionCriteria.Remove(criterion)
        );
        RecalculateStatus(deletedBy);
    }

    public void CompleteCriterion(Guid criterionId, Guid updatedBy)
    {
        var criterion = _completionCriteria.FirstOrDefault(c => c.Id == criterionId)
            ?? throw new DomainException("TICKET_COMPLETION_CRITERION_NOT_FOUND", "Ticket Completion Criterion が見つかりません");
        ChangeWithHistory(
            TicketField.CompletionCriterion,
            false,
            true,
            () => criterion.Complete(updatedBy)
        );
        RecalculateStatus(updatedBy);
    }

    public void ReopenCriterion(Guid criterionId, Guid updatedBy)
    {
        var criterion = _completionCriteria.FirstOrDefault(c => c.Id == criterionId)
            ?? throw new DomainException("TICKET_COMPLETION_CRITERION_NOT_FOUND", "Ticket Completion Criterion が見つかりません");
        ChangeWithHistory(
            TicketField.CompletionCriterion,
            true,
            false,
            () => criterion.Reopen(updatedBy)
        );
        RecalculateStatus(updatedBy);
    }

    public TicketComment AddComment(Guid authorId, string content, Guid createdBy)
    {
        var comment = new TicketComment(authorId, content, createdBy, _clock);
        _comments.Add(comment);
        return comment;
    }

    public void EditComment(
        Guid commentId, Guid authorId, string newContent, Guid updatedBy)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == commentId)
            ?? throw new DomainException("TICKET_COMMENT_NOT_FOUND", "Ticket Comment が見つかりません");

        if (comment.AuthorId != authorId)
            throw new DomainException("NOT_TICKET_COMMENT_AUTHOR", "Ticket Comment の作成者のみが編集できます");

        comment.UpdateContent(newContent, updatedBy);
    }

    public void DeleteComment(Guid commentId, Guid authorId)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == commentId)
            ?? throw new DomainException("TICKET_COMMENT_NOT_FOUND", "Ticket Comment が見つかりません");

        if (comment.AuthorId != authorId)
            throw new DomainException("NOT_TICKET_COMMENT_AUTHOR", "Ticket Comment の作成者のみが削除できます");

        _comments.Remove(comment);
    }

    private void ChangeWithHistory<T>(
        TicketField field,
        T before,
        T after,
        Action apply)
    {
        if (Equals(before, after)) return;

        apply();

        var change = TicketHistoryChange.Create(
            field,
            TicketHistoryChangeValue.From(before),
            TicketHistoryChangeValue.From(after)
        );
        _pendingChanges.Add(change);
    }

    public void CommitHistory(
        TicketHistoryAction action,
        Guid actorId)
    {
        if (!_pendingChanges.Any()) return;

        var history = new TicketHistory(
            actorId,
            _clock.Now,
            action,
            actorId,
            _clock
        );

        foreach (var change in _pendingChanges)
            history.AddChange(change.Field, change.Before, change.After);

        _histories.Add(history);
        _pendingChanges.Clear();
    }

    private void RecalculateStatus(Guid actorId)
    {
        if (_completionCriteria.Count == 0)
            return;

        if (_completionCriteria.All(c => c.IsCompleted))
        {
            if (Status.Value != TicketStatus.StatusType.Done)
                ChangeStatus(TicketStatus.StatusType.Done, actorId);
        }
        else
        {
            if (Status.Value == TicketStatus.StatusType.Done)
                ChangeStatus(TicketStatus.StatusType.InProgress, actorId);
        }
    }
}
