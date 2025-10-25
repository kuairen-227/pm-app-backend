using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Events.Projects.MemberInvited;

public sealed class MemberInvitedHandler
    : BaseEventHandler, INotificationHandler<MemberInvitedNotification>
{
    private readonly ProjectNotificationFactory _notificationFactory;
    private readonly INotificationRepository _notificationRepository;

    public MemberInvitedHandler(
        ProjectNotificationFactory notificationFactory,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : base(unitOfWork, userContext)
    {
        _notificationFactory = notificationFactory;
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(MemberInvitedNotification notification, CancellationToken cancellationToken)
    {
        var notificationEntity = _notificationFactory.CreateMemberInvitedNotification(
            notification.UserId,
            notification.ProjectId,
            notification.ProjectName,
            UserContext.Id
        );

        await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
    }
}
