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
    }

    public void Assign(Guid userId) => AssigneeId = userId;

    public void ChangeStatus(Status.StatusType status) => Status = Status.Create(status);
}
