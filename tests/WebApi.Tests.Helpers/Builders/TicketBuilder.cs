using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class TicketBuilder : BaseBuilder<TicketBuilder, Ticket>
{
    private Guid _projectId = Guid.NewGuid();
    private string _title = "デフォルトチケット";
    private string _description = "デフォルト説明文";
    private Guid? _assigneeId = null;
    private DateOnly? _startDate = null;
    private DateOnly? _endDate = null;
    private TicketStatus.StatusType _status = TicketStatus.StatusType.Todo;
    private string? _completionCriteria = null;
    private List<TicketComment> _comments = new();

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

    public TicketBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public TicketBuilder WithAssigneeId(Guid? assigneeId)
    {
        _assigneeId = assigneeId ?? Guid.NewGuid();
        return this;
    }

    public TicketBuilder WithStartDate(DateOnly startDate)
    {
        _startDate = startDate;
        return this;
    }

    public TicketBuilder WithEndDate(DateOnly endDate)
    {
        _endDate = endDate;
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

    public TicketBuilder WithComments(params TicketComment[] comments)
    {
        _comments = comments.ToList();
        return this;
    }

    public override Ticket Build()
    {
        var ticket = new Ticket(
            _projectId,
            _title,
            _description,
            _assigneeId,
            _startDate,
            _endDate,
            _completionCriteria,
            _createdBy,
            _clock
        );

        if (_status != TicketStatus.StatusType.Todo)
        {
            ticket.ChangeStatus(_status, _createdBy);
        }

        foreach (var comment in _comments)
        {
            ticket.AddComment(comment.AuthorId, comment.Content, comment.AuditInfo.CreatedBy);
        }

        return ticket;
    }
}
