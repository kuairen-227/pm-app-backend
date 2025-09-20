using MediatR;
using WebApi.Application.Common.Security;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Domain.Common.Security.Permissions;

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
