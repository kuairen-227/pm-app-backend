using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Services.NotificationFactories;

namespace WebApi.Application.Events.Tickets.MemberAssigned;

public sealed class MemberAssignedHandler
    : BaseEventHandler, INotificationHandler<MemberAssignedNotification>
{
    private readonly TicketNotificationFactory _notificationFactory;
    private readonly INotificationRepository _notificationRepository;
    private readonly IProjectRepository _projectRepository;

    public MemberAssignedHandler(
        TicketNotificationFactory notificationFactory,
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

    public async Task Handle(MemberAssignedNotification notification, CancellationToken cancellationToken)
    {
        var recipientIds = notification.NotificationRecipientIds
            .Append(notification.AssigneeId)
            .Distinct();

        var project = await _projectRepository.GetByIdAsync(notification.ProjectId)
            ?? throw new NotFoundException(nameof(Project), notification.ProjectId);

        var notificationEntities = recipientIds.Select(recipientId =>
        {
            project.EnsureMember(recipientId);
            return _notificationFactory.CreateForTicketMemberAssigned(
                recipientId,
                notification.TicketId,
                notification.TicketTitle,
                notification.AssigneeName,
                UserContext.Id
            );
        }).ToList();

        await _notificationRepository.AddRangeAsync(notificationEntities, cancellationToken);
    }
}
