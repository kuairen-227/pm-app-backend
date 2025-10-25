using AutoMapper;
using MediatR;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Tickets.ListProjectTickets;

public class ListProjectTicketsHandler : IRequestHandler<ListProjectTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public ListProjectTicketsHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TicketDto>> Handle(ListProjectTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.ListByProjectIdAsync(
            request.ProjectId,
            request.Filter?.ToSpecification(),
            cancellationToken
        );
        return _mapper.Map<IEnumerable<TicketDto>>(tickets);
    }
}
