using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.ChangeDeadline;

[RequiresPermission(TicketPermissions.Update)]
public class ChangeDeadlineCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public DateOnly? Deadline { get; }

    public ChangeDeadlineCommand(Guid projectId, Guid ticketId, DateOnly? deadline)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Deadline = deadline;
    }
}
