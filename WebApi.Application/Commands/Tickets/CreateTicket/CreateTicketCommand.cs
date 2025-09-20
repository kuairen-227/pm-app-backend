using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.CreateTicket;

[RequiresPermission(TicketPermissions.Create)]
public class CreateTicketCommand : IRequest<Guid>, IProjectScopedRequest
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
