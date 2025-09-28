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
    private readonly IUserRepository _userRepository;

    public UserRegisteredHandler(
        UserNotificationFactory notificationFactory,
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : base(unitOfWork, userContext)
    {
        _notificationFactory = notificationFactory;
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(User), notification.UserId);

        var notificationEntity = _notificationFactory.CreateForUserRegistration(
            user.Id,
            user.Id,
            user.Name,
            UserContext.Id
        );

        await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
    }
}
