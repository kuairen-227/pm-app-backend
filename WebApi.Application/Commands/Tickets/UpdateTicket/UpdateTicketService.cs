using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Commands.Tickets.UpdateTicket;

public class UpdateTicketService : BaseCommandHandler
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly TicketNotificationFactory _notificationFactory;

    public UpdateTicketService(
        ITicketRepository ticketRepository,
        IProjectRepository projectRepository,
        IUserRepository userRepository,
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
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _notificationFactory = notificationFactory;
    }

    public async Task<Unit> UpdateTicketAsync(UpdateTicketCommand request, CancellationToken cancellationToken = default)
    {
        // 1. チケット更新
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);

        if (request.Title.HasValue)
            ticket.ChangeTitle(request.Title.Value, UserContext.Id);
        if (request.Description.HasValue)
            ticket.ChangeDescription(request.Description.Value, UserContext.Id);
        if (request.AssigneeId.HasValue)
        {
            var assigneeId = request.AssigneeId.Value;
            if (assigneeId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(assigneeId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(User), assigneeId.Value);
                ticket.Assign(assigneeId.Value, user.Name, request.NotificationRecipientIds, UserContext.Id);
            }
            else
            {
                ticket.Unassign(UserContext.Id);
            }
        }
        // TODO: 見直し
        if (request.StartDate.HasValue || request.EndDate.HasValue)
        {
            var startDate = request.StartDate.Value;
            var endDate = request.EndDate.Value;
            ticket.ChangeSchedule(startDate, endDate, UserContext.Id);
        }
        if (request.Status.HasValue)
        {
            var status = TicketStatus.Parse(request.Status.Value);
            ticket.ChangeStatus(status, UserContext.Id);
        }
        // TODO: 完了条件周りは見直し
        if (request.CompletionCriteria.HasValue)
            ticket.SetCompletionCriteria(request.CompletionCriteria.Value, UserContext.Id);
        if (request.Comment.HasValue)
            ticket.AddComment(UserContext.Id, request.Comment.Value, UserContext.Id);

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
        await _notificationRepository.AddRangeAsync(notifications, cancellationToken);

        // 3. 永続化
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);
        return Unit.Value;
    }
}
