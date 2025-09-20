using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Commands.Tickets.UpdateTicket;

[RequiresPermission(TicketPermissions.Update)]
public class UpdateTicketCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public string Title { get; }

    public UpdateTicketCommand(Guid projectId, Guid ticketId, string title)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Title = title;
    }
}
