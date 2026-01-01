using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;

namespace WebApi.Application.Commands.Tickets.AddCompletionCriterion;

[RequiresPermission(TicketPermissions.Update)]
public class AddCompletionCriterionCommand : IRequest<Unit>, IProjectScopedRequest
{
    public Guid ProjectId { get; }
    public Guid TicketId { get; }
    public string Criterion { get; }

    public AddCompletionCriterionCommand(
        Guid projectId,
        Guid ticketId,
        string criterion
    )
    {
        ProjectId = projectId;
        TicketId = ticketId;
        Criterion = criterion;
    }
}
