using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Common.Pagination;
using WebApi.Application.Queries.Tickets.Dtos;

namespace WebApi.Application.Queries.Tickets.ListProjectTickets;

[RequiresPermission(TicketPermissions.View)]
public class ListProjectTicketsQuery : PagedQuery<TicketDto>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public TicketFilter? Filter { get; init; }

    public ListProjectTicketsQuery(Guid projectId)
    {
        ProjectId = projectId;
    }
}
