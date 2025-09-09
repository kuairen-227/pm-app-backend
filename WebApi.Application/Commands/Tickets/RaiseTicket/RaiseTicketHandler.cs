using MediatR;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Commands.Tickets.RaiseTicket;

public class RaiseTicketHandler : BaseHandler, IRequestHandler<RaiseTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;

    public RaiseTicketHandler(ITicketRepository ticketRepository, IUserContext userContext, IDateTimeProvider clock)
        : base(userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Guid> Handle(RaiseTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket(
            request.ProjectId,
            TicketTitle.Create(request.Title),
            request.AssigneeId,
            Deadline.CreateNullable(request.Deadline, Clock),
            request.CompletionCriteria,
            UserContext.Id,
            Clock
        );
        await _ticketRepository.AddAsync(ticket, cancellationToken);

        return ticket.Id;
    }
}
