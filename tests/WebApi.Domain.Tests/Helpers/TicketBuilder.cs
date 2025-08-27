using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Domain.Tests.Helpers;

public class TicketBuilder
{
    private Guid _projectId = Guid.NewGuid();
    private string _title = "デフォルトチケット";
    private Guid? _assigneeId = null;
    private DateTime _deadline = DateTime.UtcNow.AddDays(1);
    private Status.StatusType _status = Status.StatusType.Todo;
    private string? _completionCriteria = null;

    public TicketBuilder WithProjectId(Guid projectId)
    {
        _projectId = projectId;
        return this;
    }

    public TicketBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public TicketBuilder WithAssigneeId(Guid? assigneeId)
    {
        _assigneeId = assigneeId ?? Guid.NewGuid();
        return this;
    }

    public TicketBuilder WithDeadline(DateTime deadline)
    {
        _deadline = deadline;
        return this;
    }

    public TicketBuilder WithStatus(Status.StatusType status)
    {
        _status = status;
        return this;
    }

    public TicketBuilder WithCompletionCriteria(string? completionCriteria)
    {
        _completionCriteria = completionCriteria ?? "デフォルトの完了基準";
        return this;
    }

    public Ticket Build()
    {
        var ticket = new Ticket(
            _projectId,
            Title.Create(_title),
            Deadline.Create(_deadline)
        );

        if (_assigneeId.HasValue)
        {
            ticket.Assign(_assigneeId.Value);
        }

        if (_status != Status.StatusType.Todo)
        {
            ticket.ChangeStatus(_status);
        }

        if (!string.IsNullOrWhiteSpace(_completionCriteria))
        {
            ticket.SetCompletionCriteria(_completionCriteria);
        }

        return ticket;
    }
}
