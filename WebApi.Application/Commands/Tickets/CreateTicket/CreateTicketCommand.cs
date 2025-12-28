using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.CreateTicket;

[RequiresPermission(TicketPermissions.Create)]
public class CreateTicketCommand : IRequest<Guid>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public string Title { get; }
    public string Description { get; }
    public Guid? AssigneeId { get; }
    public DateOnly? StartDate { get; }
    public DateOnly? EndDate { get; }
    public string? CompletionCriteria { get; private set; }
    public IReadOnlyCollection<Guid> NotificationRecipientIds { get; }

    public CreateTicketCommand(
        Guid projectId,
        string title,
        string description,
        Guid? assigneeId,
        DateOnly? startDate,
        DateOnly? endDate,
        string? completionCriteria,
        IReadOnlyCollection<Guid> notificationRecipientIds
    )
    {
        ProjectId = projectId;
        Title = title;
        Description = description;
        AssigneeId = assigneeId;
        StartDate = startDate;
        EndDate = endDate;
        CompletionCriteria = completionCriteria;
        NotificationRecipientIds = notificationRecipientIds;
    }
}
