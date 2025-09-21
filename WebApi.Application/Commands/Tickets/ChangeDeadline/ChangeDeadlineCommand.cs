using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.ChangeDeadline;

[RequiresPermission(TicketPermissions.Update)]
public class ChangeDeadlineCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public DateTimeOffset? Deadline { get; }

    public ChangeDeadlineCommand(Guid projectId, Guid ticketId, DateTimeOffset? deadline)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Deadline = deadline;
    }
}
