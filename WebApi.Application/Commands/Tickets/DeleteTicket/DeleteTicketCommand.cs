using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Application.Common.Security.Permissions;

namespace WebApi.Application.Commands.Tickets.DeleteTicket;

[RequiresPermission(TicketPermissions.Delete)]
public class DeleteTicketCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public DeleteTicketCommand(Guid id)
    {
        Id = id;
    }
}
