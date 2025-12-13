using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Commands.Tickets.UpdateTicket;

public class UpdateTicketHandler : BaseCommandHandler, IRequestHandler<UpdateTicketCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly TicketNotificationFactory _notificationFactory;

    public UpdateTicketHandler(
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

    public async Task<Unit> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        // 1. タイトル・説明の更新
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);
        ticket.ChangeTitle(request.Title, UserContext.Id);
        ticket.ChangeDescription(request.Description, UserContext.Id);

        // 2. 通知作成
        var project = await _projectRepository.GetByIdAsync(ticket.ProjectId)
            ?? throw new NotFoundException(nameof(Project), ticket.ProjectId);
        var notifications = request.NotificationRecipientIds
            .Select(recipientId =>
            {
                project.EnsureMember(recipientId);
                return _notificationFactory.CreateTicketUpdatedNotification(
                    recipientId,
                    ticket.Id,
                    ticket.Title.Value,
                    UserContext.Id
                );
            }).ToList();
        await _notificationRepository.AddRangeAsync(notifications);

        // 3. 永続化
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);
        return Unit.Value;
    }
}
