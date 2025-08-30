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

        var history = AssignmentHistory.Create(assigneeId, assignedAt ?? DateTime.UtcNow);
        _assignmentHistories.Add(history);

        AssigneeId = assigneeId;
    }

    public void UnAssign()
    {
        if (AssigneeId is null)
            throw new DomainException("NOT_ASSIGNED", "現在割り当てられていません");

        AssigneeId = null;
    }

    public void ChangeStatus(TicketStatus.StatusType status) => Status = TicketStatus.Create(status);

    public void SetCompletionCriteria(string completionCriteria)
    {
        if (string.IsNullOrWhiteSpace(completionCriteria))
            throw new DomainException("COMPLETION_CRITERIA_REQUIRED", "Completion criteria は必須です");

        CompletionCriteria = completionCriteria;
    }
}
