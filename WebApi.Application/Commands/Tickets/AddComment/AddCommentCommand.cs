using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.AddComment;

[RequiresPermission(TicketPermissions.Update)]
public class AddCommentCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public string Content { get; }
    public IEnumerable<Guid> NotificationRecipientIds { get; }

    public AddCommentCommand(
        Guid projectId,
        Guid ticketId,
        string content,
        IEnumerable<Guid> notificationRecipientIds
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Content = content;
        NotificationRecipientIds = notificationRecipientIds;
    }
}
