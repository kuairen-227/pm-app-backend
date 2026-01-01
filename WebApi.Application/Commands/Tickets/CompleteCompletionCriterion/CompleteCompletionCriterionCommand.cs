using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.CompleteCompletionCriterion;

[RequiresPermission(TicketPermissions.Update)]
public class CompleteCompletionCriterionCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Guid CriterionId { get; }

    public CompleteCompletionCriterionCommand(
        Guid projectId,
        Guid ticketId,
        Guid criterionId
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        CriterionId = criterionId;
    }
}
