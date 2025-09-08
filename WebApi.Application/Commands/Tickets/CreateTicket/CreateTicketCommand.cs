using MediatR;

namespace WebApi.Application.Commands.Tickets.CreateTicket;

public class CreateTicketCommand : IRequest<Guid>
{
    public Guid ProjectId { get; }
    public string Title { get; }
    public Guid? AssigneeId { get; }
    public DateTimeOffset? Deadline { get; }
    public string? CompletionCriteria { get; private set; }

    public CreateTicketCommand(
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
