using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Events.Users.UserRoleChanged;

public sealed class UserRoleChangedHandler
    : BaseEventHandler, INotificationHandler<UserRoleChangedNotification>
{
    private readonly UserNotificationFactory _notificationFactory;
    private readonly INotificationRepository _notificationRepository;

    public UserRoleChangedHandler(
        UserNotificationFactory notificationFactory,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : base(unitOfWork, userContext)
    {
        _notificationFactory = notificationFactory;
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(UserRoleChangedNotification notification, CancellationToken cancellationToken)
    {
        var notificationEntity = _notificationFactory.CreateUserRoleChangedNotification(
            notification.UserId,
            notification.UserId,
            notification.Role,
            UserContext.Id
        );

        await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
    }
}
