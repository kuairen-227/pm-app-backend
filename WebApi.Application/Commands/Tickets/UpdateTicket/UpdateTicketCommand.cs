using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Application.Commands.Tickets.UpdateTicket;

[RequiresPermission(TicketPermissions.Update)]
public class UpdateTicketCommand : IRequest<Unit>
{
    public Guid Id { get; }
    public string Title { get; }

    public UpdateTicketCommand(Guid id, string title)
    {
        Id = id;
        Title = title;
    }
}
