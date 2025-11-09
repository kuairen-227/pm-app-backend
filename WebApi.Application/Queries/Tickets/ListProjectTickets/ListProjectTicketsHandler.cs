using AutoMapper;
using MediatR;
using WebApi.Application.Common.Pagination;
using WebApi.Application.Queries.Tickets.Dtos;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Queries.Tickets.ListProjectTickets;

public class ListProjectTicketsHandler : IRequestHandler<ListProjectTicketsQuery, PagedResultDto<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;

    public ListProjectTicketsHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<TicketDto>> Handle(ListProjectTicketsQuery request, CancellationToken cancellationToken)
    {
        var result = await _ticketRepository.ListByProjectIdAsync(
            request.ProjectId,
            request.Filter?.ToSpecification(),
            request.Pagination.Skip,
            request.Pagination.PageSize,
            request.Sorting.SortBy,
            request.Sorting.SortOrder,
            cancellationToken
        );

        return new PagedResultDto<TicketDto>
        {
            Items = _mapper.Map<IReadOnlyList<TicketDto>>(result.Items),
            TotalCount = result.TotalCount,
            PageNumber = request.Pagination.PageNumber,
            PageSize = request.Pagination.PageSize
        };
    }
}
