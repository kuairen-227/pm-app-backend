using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Notifications.MarkNotificationAsRead;

[RequiresPermission(NotificationPermissions.Read)]
public class MarkNotificationAsReadCommand : IRequest<Unit>
{
    public Guid NotificationId { get; }

    public MarkNotificationAsReadCommand(Guid notificationId)
    {
        NotificationId = notificationId;
    }
}
