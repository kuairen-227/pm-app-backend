using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Helpers.Common;

namespace WebApi.Domain.Tests.Helpers;

public class TicketBuilder : BaseBuilder<TicketBuilder, Ticket>
{
    private Guid _projectId = Guid.NewGuid();
    private string _title = "デフォルトチケット";
    private Guid? _assigneeId = null;
    private DateTimeOffset? _deadline = null;
    private TicketStatus.StatusType _status = TicketStatus.StatusType.Todo;
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

    public TicketBuilder WithDeadline(DateTimeOffset deadline)
    {
        _deadline = deadline;
        return this;
    }

    public TicketBuilder WithStatus(TicketStatus.StatusType status)
    {
        _status = status;
        return this;
    }

    public TicketBuilder WithCompletionCriteria(string? completionCriteria)
    {
        _completionCriteria = completionCriteria ?? "デフォルトの完了基準";
        return this;
    }

    public override Ticket Build()
    {
        var ticket = new Ticket(
            _projectId,
            TicketTitle.Create(_title),
            _assigneeId,
            Deadline.CreateNullable(_deadline, _clock),
            _completionCriteria,
            _createdBy,
            _clock
        );

        if (_status != TicketStatus.StatusType.Todo)
        {
            ticket.ChangeStatus(_status);
        }

        return ticket;
    }
}
