using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;

namespace WebApi.Application.Commands.Notifications.MarkNotificationAsRead;

public class MarkNotificationAsReadHandler : BaseCommandHandler, IRequestHandler<MarkNotificationAsReadCommand, Unit>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationAsReadHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Unit> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId)
            ?? throw new NotFoundException(nameof(Notification), request.NotificationId);

        notification.MarkAsRead(UserContext.Id);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return Unit.Value;
    }
}
