using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class TicketBuilder : BaseBuilder<TicketBuilder, Ticket>
{
    private Guid _projectId = Guid.NewGuid();
    private string _title = "デフォルトチケット";
    private Guid? _assigneeId = null;
    private DateOnly? _deadline = null;
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

    public TicketBuilder WithDeadline(DateOnly deadline)
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
            _title,
            _assigneeId,
            _deadline,
            _completionCriteria,
            _createdBy,
            _clock
        );

        if (_status != TicketStatus.StatusType.Todo)
        {
            ticket.ChangeStatus(_status, _createdBy, _clock);
        }

        return ticket;
    }
}
