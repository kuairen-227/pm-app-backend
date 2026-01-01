using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.EditCompletionCriterion;

[RequiresPermission(TicketPermissions.Update)]
public class EditCompletionCriterionCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public Guid CriterionId { get; }
    public string Criterion { get; }

    public EditCompletionCriterionCommand(
        Guid projectId,
        Guid ticketId,
        Guid criterionId,
        string criterion
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        CriterionId = criterionId;
        Criterion = criterion;
    }
}
