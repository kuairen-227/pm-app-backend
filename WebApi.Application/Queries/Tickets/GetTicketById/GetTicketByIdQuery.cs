using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Queries.Tickets.Dtos;

namespace WebApi.Application.Queries.Tickets.GetTicketById;

[RequiresPermission(TicketPermissions.View)]
public class GetTicketByIdQuery : IRequest<TicketDetailDto>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }

    public GetTicketByIdQuery(Guid projectId, Guid ticketId)
    {
        ProjectId = projectId;
        TicketId = ticketId;
    }
}
