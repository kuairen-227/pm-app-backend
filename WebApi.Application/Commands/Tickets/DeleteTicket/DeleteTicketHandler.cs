using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Tickets.DeleteTicket;

public class DeleteTicketHandler : BaseCommandHandler, IRequestHandler<DeleteTicketCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;

    public DeleteTicketHandler(
        ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IUserContext userContext, IDateTimeProvider clock)
        : base(unitOfWork, userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("TICKET_NOT_FOUND", "Ticket が見つかりません");

        await _ticketRepository.DeleteAsync(ticket, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
