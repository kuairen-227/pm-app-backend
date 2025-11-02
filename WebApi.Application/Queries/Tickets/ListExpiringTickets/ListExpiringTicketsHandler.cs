using AutoMapper;
using MediatR;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Tickets.ListExpiringTickets;

public class ListExpiringTicketsHandler : IRequestHandler<ListExpiringTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public ListExpiringTicketsHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TicketDto>> Handle(ListExpiringTicketsQuery request, CancellationToken cancellationToken)
    {
        var dueWithin = request.DueWithin ?? TimeSpan.FromDays(7);

        var tickets = await _ticketRepository.ListExpiringTicketsByAssigneeIdAsync(
            request.UserId, dueWithin, cancellationToken);
        return _mapper.Map<IEnumerable<TicketDto>>(tickets);
    }
}
