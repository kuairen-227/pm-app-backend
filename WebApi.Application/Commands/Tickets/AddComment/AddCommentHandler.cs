using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Commands.Tickets.AddComment;

public class AddCommentHandler : BaseCommandHandler, IRequestHandler<AddCommentCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;

    public AddCommentHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        ticket.AddComment(UserContext.Id, request.Content, UserContext.Id, Clock);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return Unit.Value;
    }
}
