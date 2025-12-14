using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Events.Users.UserUpdated;

public sealed class UserUpdatedHandler
    : BaseEventHandler, INotificationHandler<UserUpdatedNotification>
{
    private readonly UserNotificationFactory _notificationFactory;
    private readonly INotificationRepository _notificationRepository;

    public UserUpdatedHandler(
        UserNotificationFactory notificationFactory,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : base(unitOfWork, userContext)
    {
        _notificationFactory = notificationFactory;
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(UserUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var notificationEntity = _notificationFactory.CreateUserUpdatedNotification(
            notification.UserId,
            notification.UserId,
            UserContext.Id
        );

        await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
    }
}
