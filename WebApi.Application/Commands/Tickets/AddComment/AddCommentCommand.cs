using MediatR;
using WebApi.Application.Common;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.AddComment;

[RequiresPermission(TicketPermissions.Update)]
public class AddCommentCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Optional<Guid?> AssigneeId { get; }
    public Optional<DateOnly?> StartDate { get; }
    public Optional<DateOnly?> EndDate { get; }
    public Optional<string> Status { get; }
    public Optional<string> Comment { get; }
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; } = Array.Empty<Guid>();

    public AddCommentCommand(
        Guid projectId,
        Guid ticketId,
        Optional<Guid?> assigneeId,
        Optional<DateOnly?> startDate,
        Optional<DateOnly?> endDate,
        Optional<string> status,
        Optional<string> comment,
        IReadOnlyCollection<Guid> notificationRecipientIds
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        AssigneeId = assigneeId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        Comment = comment;
        NotificationRecipientIds = notificationRecipientIds;
    }
}
