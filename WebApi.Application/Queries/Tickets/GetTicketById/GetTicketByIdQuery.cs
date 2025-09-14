using MediatR;

namespace WebApi.Application.Queries.Tickets.GetTicketById;

public class GetTicketByIdQuery : IRequest<TicketDetailDto?>
{
    public Guid Id { get; }

    public GetTicketByIdQuery(Guid id)
    {
        Id = id;
    }
}
