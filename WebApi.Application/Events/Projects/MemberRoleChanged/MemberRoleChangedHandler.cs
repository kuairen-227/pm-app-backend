using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Events.Projects.MemberRoleChanged;

public sealed class MemberRoleChangedHandler
    : BaseEventHandler, INotificationHandler<MemberRoleChangedNotification>
{
    private readonly ProjectNotificationFactory _notificationFactory;
    private readonly INotificationRepository _notificationRepository;
    private readonly IProjectRepository _projectRepository;

    public MemberRoleChangedHandler(
        ProjectNotificationFactory notificationFactory,
        INotificationRepository notificationRepository,
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    ) : base(unitOfWork, userContext)
    {
        _notificationFactory = notificationFactory;
        _notificationRepository = notificationRepository;
        _projectRepository = projectRepository;
    }

    public async Task Handle(MemberRoleChangedNotification notification, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(notification.ProjectId, cancellationToken)
            ?? throw new NotFoundException(nameof(Project), notification.ProjectId);

        var notificationEntity = _notificationFactory.CreateForProjectInvitation(
            notification.UserId,
            notification.ProjectId,
            project.Name,
            UserContext.Id
        );

        await _notificationRepository.AddAsync(notificationEntity, cancellationToken);
    }
}
