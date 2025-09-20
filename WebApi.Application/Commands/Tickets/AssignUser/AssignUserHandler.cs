using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Tickets.AssignUser;

public class AssignUserHandler : BaseCommandHandler, IRequestHandler<AssignUserCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public AssignUserHandler(
        ITicketRepository ticketRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTimeProvider clock)
        : base(unitOfWork, userContext, clock)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(AssignUserCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException("TICKET_NOT_FOUND", "Ticket が見つかりません");
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("USER_NOT_FOUND", "User が見つかりません");

        // TODO: アサイン可能かどうかのチェック（ex: プロジェクトに所属するユーザーか）

        ticket.Assign(user.Id, Clock);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
