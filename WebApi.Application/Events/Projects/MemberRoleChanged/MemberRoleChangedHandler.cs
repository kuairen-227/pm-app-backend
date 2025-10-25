using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Events.Projects.MemberRoleChanged;

public sealed class MemberRoleChangedHandler
    : BaseEventHandler, INotificationHandler<MemberRoleChangedNotification>
{
    private readonly ProjectNotificationFactory _notificationFactory;
    private readonly INotificationRepository _notificationRepository;

    public MemberRoleChangedHandler(
        ProjectNotificationFactory notificationFactory,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : base(unitOfWork, userContext)
    {
        _notificationFactory = notificationFactory;
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(MemberRoleChangedNotification notification, CancellationToken cancellationToken)
    {
        var notificationEntity = _notificationFactory.CreateProjectRoleChangedNotification(
            notification.UserId,
            notification.ProjectId,
            notification.Role,
            UserContext.Id
        );

        await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
    }
}
