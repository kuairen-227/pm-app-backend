using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Commands.Tickets.CreateTicket;

public class CreateTicketHandler : BaseCommandHandler, IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly TicketNotificationFactory _notificationFactory;

    public CreateTicketHandler(
        ITicketRepository ticketRepository,
        IProjectRepository projectRepository,
        INotificationRepository notificationRepository,
        TicketNotificationFactory notificationFactory,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _ticketRepository = ticketRepository;
        _projectRepository = projectRepository;
        _notificationRepository = notificationRepository;
        _notificationFactory = notificationFactory;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        // 1. チケット作成
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

        // 2. 通知作成
        var project = await _projectRepository.GetByIdAsync(ticket.ProjectId)
            ?? throw new NotFoundException(nameof(Project), ticket.ProjectId);
        var notifications = request.NotificationRecipientIds
            .Select(recipientId =>
            {
                project.EnsureMember(recipientId);
                return _notificationFactory.CreateForTicketCreation(
                    recipientId,
                    ticket.Id,
                    ticket.Title,
                    UserContext.Id
                );
            }).ToList();

        await _notificationRepository.AddRangeAsync(notifications);

        // 3. 永続化
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);
        return ticket.Id;
    }
}
