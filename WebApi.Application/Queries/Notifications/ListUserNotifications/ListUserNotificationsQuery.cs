using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Common.Pagination;
using WebApi.Application.Queries.Notifications.Dtos;

namespace WebApi.Application.Queries.Notifications.ListUserNotifications;

[RequiresPermission(NotificationPermissions.View)]
public class ListUserNotificationsQuery : PagedQuery<NotificationDto>
{
    public Guid RecipientId { get; }

    public ListUserNotificationsQuery(Guid recipientId)
    {
        RecipientId = recipientId;
    }
}
