using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.EditComment;

[RequiresPermission(TicketPermissions.Update)]
public class EditCommentCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Guid CommentId { get; }
    public string Content { get; }

    public EditCommentCommand(Guid projectId, Guid ticketId, Guid commentId, string content)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        CommentId = commentId;
        Content = content;
    }
}
