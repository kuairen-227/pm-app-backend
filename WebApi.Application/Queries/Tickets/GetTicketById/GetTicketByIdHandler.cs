using AutoMapper;
using MediatR;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Tickets.GetTicketById;

public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdQuery, TicketDetailDto?>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public GetTicketByIdHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<TicketDetailDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null) return null;

        return _mapper.Map<TicketDetailDto>(ticket);
    }
}
