using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Tickets.AssignMember;

public class AssignMemberHandler : BaseCommandHandler, IRequestHandler<AssignMemberCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectRepository _projectRepository;

    public AssignMemberHandler(
        ITicketRepository ticketRepository,
        IUserRepository userRepository,
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IDateTimeProvider clock)
        : base(unitOfWork, userContext, clock)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Unit> Handle(AssignMemberCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException("TICKET_NOT_FOUND", "Ticket が見つかりません");
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new NotFoundException("USER_NOT_FOUND", "User が見つかりません");

        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken)
            ?? throw new NotFoundException("PROJECT_NOT_FOUND", "Project が見つかりません");
        project.EnsureMember(user.Id);

        ticket.Assign(user.Id, UserContext.Id, Clock);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
