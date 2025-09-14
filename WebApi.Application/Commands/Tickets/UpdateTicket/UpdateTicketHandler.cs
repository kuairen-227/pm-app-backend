using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Commands.Tickets.UpdateTicket;

public class UpdateTicketHandler : BaseHandler, IRequestHandler<UpdateTicketCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;

    public UpdateTicketHandler(ITicketRepository ticketRepository, IUserContext userContext, IDateTimeProvider clock)
        : base(userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Unit> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("TICKET_NOT_FOUND", "Ticket が見つかりません");

        var title = TicketTitle.Create(request.Title);
        ticket.ChangeTitle(title);

        await _ticketRepository.UpdateAsync(ticket, cancellationToken);

        return Unit.Value;
    }
}
