using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Commands.Tickets.ChangeDeadline;

public class ChangeDeadlineHandler : BaseCommandHandler, IRequestHandler<ChangeDeadlineCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;

    public ChangeDeadlineHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Unit> Handle(ChangeDeadlineCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        var deadline = request.Deadline.HasValue
            ? Deadline.Create(request.Deadline.Value, Clock)
            : null;
        ticket.ChangeDeadline(deadline, UserContext.Id, Clock);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
