using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Events.Users.UserRegistered;

public sealed class UserRegisteredHandler
    : BaseEventHandler, INotificationHandler<UserRegisteredNotification>
{
    private readonly UserNotificationFactory _notificationFactory;
    private readonly INotificationRepository _notificationRepository;

    public UserRegisteredHandler(
        UserNotificationFactory notificationFactory,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : base(unitOfWork, userContext)
    {
        _notificationFactory = notificationFactory;
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        var notificationEntity = _notificationFactory.CreateUserRegisteredNotification(
            notification.UserId,
            notification.UserId,
            notification.UserName,
            UserContext.Id
        );

        await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
    }
}
