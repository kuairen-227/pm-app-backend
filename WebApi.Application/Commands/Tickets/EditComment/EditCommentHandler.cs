using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Commands.Tickets.EditComment;

public class EditCommentHandler : BaseCommandHandler, IRequestHandler<EditCommentCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;

    public EditCommentHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Unit> Handle(EditCommentCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        ticket.EditComment(request.CommentId, UserContext.Id, request.Content, UserContext.Id);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return Unit.Value;
    }
}
