using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.TicketAggregate;

public sealed class Ticket : Entity
{
    public Guid ProjectId { get; private set; }
    public TicketTitle Title { get; private set; } = null!;
    public Guid? AssigneeId { get; private set; }
    public Deadline? Deadline { get; private set; } = null!;
    public TicketStatus Status { get; private set; } = null!;
    public string? CompletionCriteria { get; private set; }

    private readonly List<TicketComment> _comments = new();
    public IReadOnlyList<TicketComment> Comments => _comments.AsReadOnly();

    private readonly List<AssignmentHistory> _assignmentHistories = new();
    public IReadOnlyList<AssignmentHistory> AssignmentHistories => _assignmentHistories.AsReadOnly();

    private Ticket() { } // EF Core 用

    public Ticket(
        Guid projectId,
        string title,
        Guid? assigneeId,
        DateOnly? deadline,
        string? completionCriteria,
        Guid createdBy,
        IDateTimeProvider clock
    ) : base(createdBy, clock)
    {
        if (projectId == Guid.Empty)
            throw new DomainException("PROJECT_ID_REQUIRED", "Project ID は必須です");

        ProjectId = projectId;
        Title = TicketTitle.Create(title);
        AssigneeId = assigneeId;
        Deadline = Deadline.CreateNullable(deadline, clock);
        Status = TicketStatus.Create(TicketStatus.StatusType.Todo);
        CompletionCriteria = completionCriteria;
    }

    public void ChangeTitle(string title, Guid updatedBy, IDateTimeProvider clock)
    {
        Title = TicketTitle.Create(title);
        UpdateAuditInfo(updatedBy, clock);
    }

    public void Assign(Guid assigneeId, Guid updatedBy, IDateTimeProvider clock)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");
        if (AssigneeId == assigneeId)
            throw new DomainException("ALREADY_ASSIGNED_SAME_USER", "既に同じユーザーに割り当てられています");

        AssignmentHistory history;
        if (AssigneeId is null)
        {
            history = AssignmentHistory.Assigned(assigneeId, clock.Now);
        }
        else
        {
            history = AssignmentHistory.Changed(assigneeId, AssigneeId.Value, clock.Now);
        }

        _assignmentHistories.Add(history);
        AssigneeId = assigneeId;

        UpdateAuditInfo(updatedBy, clock);
    }

    public void Unassign(Guid updatedBy, IDateTimeProvider clock)
    {
        if (AssigneeId is null)
            throw new DomainException("NOT_ASSIGNED", "現在割り当てられていません");

        var history = AssignmentHistory.Unassigned(AssigneeId.Value, clock.Now);
        _assignmentHistories.Add(history);
        AssigneeId = null;

        UpdateAuditInfo(updatedBy, clock);
    }

    public void ChangeDeadline(DateOnly? newDeadline, Guid updatedBy, IDateTimeProvider clock)
    {
        Deadline = Deadline.CreateNullable(newDeadline, clock);
        UpdateAuditInfo(updatedBy, clock);
    }

    public void ChangeStatus(TicketStatus.StatusType status, Guid updatedBy, IDateTimeProvider clock)
    {
        Status = TicketStatus.Create(status);
        UpdateAuditInfo(updatedBy, clock);
    }

    public void SetCompletionCriteria(string completionCriteria, Guid updatedBy, IDateTimeProvider clock)
    {
        if (string.IsNullOrWhiteSpace(completionCriteria))
            throw new DomainException("COMPLETION_CRITERIA_REQUIRED", "Completion criteria は必須です");

        CompletionCriteria = completionCriteria;
        UpdateAuditInfo(updatedBy, clock);
    }

    public TicketComment AddComment(Guid authorId, string content, Guid createdBy, IDateTimeProvider clock)
    {
        var comment = TicketComment.Create(Id, authorId, content, createdBy, clock);
        _comments.Add(comment);
        return comment;
    }

    public void EditComment(
        Guid commentId, Guid authorId, string newContent, Guid updatedBy, IDateTimeProvider clock)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == commentId)
            ?? throw new DomainException("TICKET_COMMENT_NOT_FOUND", "Ticket Comment が見つかりません");

        if (comment.AuthorId != authorId)
            throw new DomainException("NOT_TICKET_COMMENT_AUTHOR", "Ticket Comment の作成者のみが編集できます");

        comment.UpdateContent(newContent, updatedBy, clock);
    }

    public void RemoveComment(Guid commentId, Guid authorId)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == commentId)
            ?? throw new DomainException("TICKET_COMMENT_NOT_FOUND", "Ticket Comment が見つかりません");

        if (comment.AuthorId != authorId)
            throw new DomainException("NOT_TICKET_COMMENT_AUTHOR", "Ticket Comment の作成者のみが削除できます");

        _comments.Remove(comment);
    }
}
