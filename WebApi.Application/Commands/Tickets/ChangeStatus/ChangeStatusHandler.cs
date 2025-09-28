using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Commands.Tickets.ChangeStatus;

public class ChangeStatusHandler : BaseCommandHandler, IRequestHandler<ChangeStatusCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;

    public ChangeStatusHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Unit> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        ticket.ChangeStatus(request.Status, UserContext.Id);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return Unit.Value;
    }
}
