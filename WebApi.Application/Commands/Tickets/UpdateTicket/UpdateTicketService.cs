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
        var project = await _projectRepository.GetByIdAsync(ticket.ProjectId)
            ?? throw new NotFoundException(nameof(Project), ticket.ProjectId);

        // タイトル
        if (request.Title.TryGetValue(out var title))
            ticket.ChangeTitle(title, UserContext.Id);

        // 説明
        if (request.Description.TryGetValue(out var description))
            ticket.ChangeDescription(description, UserContext.Id);

        // 担当者
        if (request.AssigneeId.TryGetValue(out var assigneeId))
        {
            if (assigneeId.HasValue)
            {
                var user = await _userRepository.GetByIdAsync(assigneeId.Value, cancellationToken)
                    ?? throw new NotFoundException(nameof(User), assigneeId.Value);
                project.EnsureMember(assigneeId.Value);
                ticket.Assign(assigneeId.Value, user.Name, request.NotificationRecipientIds, UserContext.Id);
            }
            else
            {
                ticket.Unassign(UserContext.Id);
            }
        }

        // スケジュール
        DateOnly? startDate = null;
        DateOnly? endDate = null;

        request.StartDate.TryGetValue(out startDate);
        request.EndDate.TryGetValue(out endDate);

        if (startDate.HasValue || endDate.HasValue)
        {
            var s = startDate ?? ticket.Schedule.StartDate;
            var e = endDate ?? ticket.Schedule.EndDate;
            ticket.ChangeSchedule(s, e, UserContext.Id);
        }

        // ステータス
        if (request.Status.TryGetValue(out var statusString))
        {
            var status = TicketStatus.Parse(statusString);
            ticket.ChangeStatus(status, UserContext.Id);
        }

        // 完了条件
        if (request.CompletionCriterionOperations.TryGetValue(out var operations))
        {
            foreach (var operation in operations)
            {
                switch (operation)
                {
                    case AddCompletionCriterionOperationDto add:
                        ticket.AddCompletionCriterion(add.Criterion, UserContext.Id);
                        break;
                    case EditCompletionCriterionOperationDto edit:
                        ticket.EditCompletionCriterion(edit.CriterionId, edit.Criterion, UserContext.Id);
                        break;
                    case DeleteCompletionCriterionOperationDto delete:
                        ticket.DeleteCompletionCriterion(delete.CriterionId, UserContext.Id);
                        break;
                    case CompleteCompletionCriterionOperationDto complete:
                        ticket.CompleteCriterion(complete.CriterionId, UserContext.Id);
                        break;
                    case ReopenCompletionCriterionOperationDto reopen:
                        ticket.ReopenCriterion(reopen.CriterionId, UserContext.Id);
                        break;
                }
            }
        }

        // コメント
        if (request.Comment.TryGetValue(out var comment))
            ticket.AddComment(UserContext.Id, comment, UserContext.Id);

        // 2. 通知作成
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
