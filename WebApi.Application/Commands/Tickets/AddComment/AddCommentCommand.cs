using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.AddComment;

[RequiresPermission(TicketPermissions.Update)]
public class AddCommentCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public string Content { get; }

    public AddCommentCommand(Guid projectId, Guid ticketId, string content)
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Content = content;
    }
}
