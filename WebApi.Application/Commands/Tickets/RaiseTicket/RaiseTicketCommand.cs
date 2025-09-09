using MediatR;

namespace WebApi.Application.Commands.Tickets.RaiseTicket;

public class RaiseTicketCommand : IRequest<Guid>
{
    public Guid ProjectId { get; }
    public string Title { get; }
    public Guid? AssigneeId { get; }
    public DateTimeOffset? Deadline { get; }
    public string? CompletionCriteria { get; private set; }

    public RaiseTicketCommand(
        Guid projectId, string title, Guid? assigneeId, DateTimeOffset? deadline, string? completionCriteria
    )
    {
        ProjectId = projectId;
        Title = title;
        AssigneeId = assigneeId;
        Deadline = deadline;
        CompletionCriteria = completionCriteria;
    }
}
