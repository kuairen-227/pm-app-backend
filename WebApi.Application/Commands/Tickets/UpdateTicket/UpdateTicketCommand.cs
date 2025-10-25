using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.UpdateTicket;

[RequiresPermission(TicketPermissions.Update)]
public class UpdateTicketCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public string Title { get; }
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; } = Array.Empty<Guid>();

    public UpdateTicketCommand(
        Guid projectId,
        Guid ticketId,
        string title,
        IReadOnlyCollection<Guid> notificationRecipientIds
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Title = title;
        NotificationRecipientIds = notificationRecipientIds;
    }
}
