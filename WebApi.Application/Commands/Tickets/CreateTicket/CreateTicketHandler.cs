using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Commands.Tickets.CreateTicket;

public class CreateTicketHandler : BaseCommandHandler, IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket(
            request.ProjectId,
            request.Title,
            request.AssigneeId,
            request.Deadline,
            request.CompletionCriteria,
            UserContext.Id,
            Clock
        );

        await _ticketRepository.AddAsync(ticket, cancellationToken);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return ticket.Id;
    }
}
