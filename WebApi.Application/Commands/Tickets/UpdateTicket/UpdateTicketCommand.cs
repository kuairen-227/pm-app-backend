using MediatR;
using WebApi.Application.Common;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.UpdateTicket;

[RequiresPermission(TicketPermissions.Update)]
public class UpdateTicketCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Optional<string> Title { get; }
    public Optional<string> Description { get; }
    public Optional<Guid?> AssigneeId { get; init; }
    public Optional<DateOnly?> StartDate { get; init; }
    public Optional<DateOnly?> EndDate { get; init; }
    public Optional<string> Status { get; init; }
    public Optional<IReadOnlyList<ICompletionCriterionOperationDto>> CompletionCriterionOperations { get; init; }
    public Optional<string> Comment { get; init; }
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; } = Array.Empty<Guid>();

    public UpdateTicketCommand(
        Guid projectId,
        Guid ticketId,
        Optional<string> title,
        Optional<string> description,
        Optional<Guid?> assigneeId,
        Optional<DateOnly?> startDate,
        Optional<DateOnly?> endDate,
        Optional<string> status,
        Optional<IReadOnlyList<ICompletionCriterionOperationDto>> completionCriterionOperations,
        Optional<string> comment,
        IReadOnlyCollection<Guid> notificationRecipientIds
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Title = title;
        Description = description;
        AssigneeId = assigneeId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        CompletionCriterionOperations = completionCriterionOperations;
        Comment = comment;
        NotificationRecipientIds = notificationRecipientIds;
    }
}
