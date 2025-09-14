using MediatR;
using WebApi.Application.Queries.Tickets.Dtos;

namespace WebApi.Application.Queries.Tickets.ListProjectTickets;

public class ListProjectTicketsQuery : IRequest<IEnumerable<TicketDto>>
{
    public Guid ProjectId { get; }

    public ListProjectTicketsQuery(Guid projectId)
    {
        ProjectId = projectId;
    }
}
