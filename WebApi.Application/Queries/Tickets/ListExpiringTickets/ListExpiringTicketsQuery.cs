using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Queries.Tickets.Dtos;

namespace WebApi.Application.Queries.Tickets.ListExpiringTickets;

[RequiresPermission(TicketPermissions.View)]
public class ListExpiringTicketsQuery : IRequest<IEnumerable<TicketDto>>
{
    public Guid UserId { get; }
    public TimeSpan? DueWithin { get; init; }

    public ListExpiringTicketsQuery(Guid userId, TimeSpan? dueWithin = null)
    {
        UserId = userId;
        DueWithin = dueWithin;
    }
}
