using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.DeleteComment;

[RequiresPermission(TicketPermissions.Delete)]
public class DeleteCommentCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Guid CommentId { get; }

    public DeleteCommentCommand(Guid projectId, Guid ticketId, Guid commentId)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        CommentId = commentId;
    }
}
