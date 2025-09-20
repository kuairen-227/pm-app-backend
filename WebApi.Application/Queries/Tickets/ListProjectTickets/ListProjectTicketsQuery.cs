using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Queries.Tickets.ListProjectTickets;

[RequiresPermission(TicketPermissions.View)]
public class ListProjectTicketsQuery : IRequest<IEnumerable<TicketDto>>, IProjectScopedRequest
{
    public Guid ProjectId { get; }

    public ListProjectTicketsQuery(Guid projectId)
    {
        ProjectId = projectId;
    }
}
