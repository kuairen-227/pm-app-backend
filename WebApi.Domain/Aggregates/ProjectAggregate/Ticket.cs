using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Ticket : Entity
{
    public Guid ProjectId { get; private set; }
    public TicketTitle Title { get; private set; } = null!;
    public Guid? AssigneeId { get; private set; }
    public Deadline Deadline { get; private set; } = null!;
    public TicketStatus Status { get; private set; } = null!;
    public string? CompletionCriteria { get; private set; }

    private readonly List<TicketComment> _comments = new();
    public IReadOnlyList<TicketComment> Comments => _comments.AsReadOnly();

    private readonly List<AssignmentHistory> _assignmentHistories = new();
    public IReadOnlyList<AssignmentHistory> AssignmentHistories => _assignmentHistories.AsReadOnly();

    private Ticket() { } // EF Core 用

    public Ticket(Guid projectId, TicketTitle title, Deadline deadline)
    {
        if (projectId == Guid.Empty)
            throw new DomainException("PROJECT_ID_REQUIRED", "Project ID は必須です");

        ProjectId = projectId;
        Title = title;
        Deadline = deadline;
        Status = TicketStatus.Create(TicketStatus.StatusType.Todo);
    }

    public void Assign(Guid assigneeId, DateTime? assignedAt = null)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");
        if (AssigneeId == assigneeId)
            throw new DomainException("ALREADY_ASSIGNED_SAME_USER", "既に同じユーザーに割り当てられています");

        AssignmentHistory history;
        var changedAt = assignedAt ?? DateTime.UtcNow;
        if (AssigneeId is null)
        {
            history = AssignmentHistory.Assigned(assigneeId, changedAt);
        }
        else
        {
            history = AssignmentHistory.Changed(assigneeId, AssigneeId.Value, changedAt);
        }

        _assignmentHistories.Add(history);
        AssigneeId = assigneeId;
    }

    public void UnAssign(DateTime? unassignedAt = null)
    {
        if (AssigneeId is null)
            throw new DomainException("NOT_ASSIGNED", "現在割り当てられていません");

        var history = AssignmentHistory.Unassigned(AssigneeId.Value, unassignedAt ?? DateTime.UtcNow);
        _assignmentHistories.Add(history);
        AssigneeId = null;
    }

    public void ChangeStatus(TicketStatus.StatusType status) => Status = TicketStatus.Create(status);

    public void SetCompletionCriteria(string completionCriteria)
    {
        if (string.IsNullOrWhiteSpace(completionCriteria))
            throw new DomainException("COMPLETION_CRITERIA_REQUIRED", "Completion criteria は必須です");

        CompletionCriteria = completionCriteria;
    }

    public TicketComment AddComment(Guid authorId, string content)
    {
        var comment = TicketComment.Create(Id, authorId, content);
        _comments.Add(comment);
        return comment;
    }

    public void EditComment(Guid commentId, Guid authorId, string newContent)
    {
        var comment = _comments.FirstOrDefault(c => c.Id == commentId)
            ?? throw new DomainException("TICKET_COMMENT_NOT_FOUND", "Ticket Comment が見つかりません");

        if (comment.AuthorId != authorId)
            throw new DomainException("NOT_TICKET_COMMENT_AUTHOR", "Ticket Comment の作成者のみが編集できます");

        comment.UpdateContent(newContent);
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
