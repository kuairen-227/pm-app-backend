using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Ticket : Entity
{
    public Guid ProjectId { get; private set; }
    public Title Title { get; private set; } = null!;
    public Guid? AssigneeId { get; private set; }
    public Deadline Deadline { get; private set; } = null!;
    public Status Status { get; private set; } = null!;
    public string? CompletionCriteria { get; private set; }

    private Ticket() { } // EF Core 用

    public Ticket(Guid projectId, Title title, Deadline deadline)
    {
        ProjectId = projectId;
        Title = title;
        Deadline = deadline;
        Status = Status.Create(Status.StatusType.Todo);
    }

    public void Assign(Guid assigneeId)
    {
        if (assigneeId == Guid.Empty)
            throw new DomainException("ASSIGNEE_ID_REQUIRED", "Assignee ID は必須です");

        AssigneeId = assigneeId;
    }

    public void ChangeStatus(Status.StatusType status) => Status = Status.Create(status);

    public void SetCompletionCriteria(string completionCriteria)
    {
        if (string.IsNullOrWhiteSpace(completionCriteria))
            throw new DomainException("COMPLETION_CRITERIA_REQUIRED", "Completion criteria は必須です");

        CompletionCriteria = completionCriteria;
    }
}
