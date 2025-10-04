using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Commands.Tickets.AddComment;

public class AddCommentHandler : BaseCommandHandler, IRequestHandler<AddCommentCommand, Unit>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly TicketNotificationFactory _notificationFactory;

    public AddCommentHandler(
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

    public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        // 1. コメント追加
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken)
            ?? throw new NotFoundException(nameof(Ticket), request.TicketId);
        ticket.AddComment(UserContext.Id, request.Content, UserContext.Id);

        // 2. 通知作成
        var project = await _projectRepository.GetByIdAsync(ticket.ProjectId)
            ?? throw new NotFoundException(nameof(Project), ticket.ProjectId);
        foreach (var recipientId in request.NotificationRecipientIds)
        {
            project.EnsureMember(recipientId);
            var notification = _notificationFactory.CreateForCommentAdded(
                recipientId,
                ticket.Id,
                ticket.Title.Value,
                UserContext.Id
            );
            await _notificationRepository.AddAsync(notification, cancellationToken);
        }

        // 3. 永続化
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);
        return Unit.Value;
    }
}
