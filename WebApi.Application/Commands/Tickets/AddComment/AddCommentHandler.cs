using MediatR;
using WebApi.Application.Commands.Tickets.UpdateTicket;
using WebApi.Application.Common;

namespace WebApi.Application.Commands.Tickets.AddComment;

public class AddCommentHandler : IRequestHandler<AddCommentCommand, Unit>
{
    private readonly UpdateTicketService _service;

    public AddCommentHandler(UpdateTicketService service)
    {
        _service = service;
    }

    public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var command = new UpdateTicketCommand(
            projectId: request.ProjectId,
            ticketId: request.TicketId,
            title: Optional<string>.None(),
            description: Optional<string>.None(),
            assigneeId: request.AssigneeId,
            startDate: request.StartDate,
            endDate: request.EndDate,
            status: request.Status,
            completionCriterionOperations: Optional<IReadOnlyList<ICompletionCriterionOperationDto>>.None(),
            comment: request.Comment,
            notificationRecipientIds: request.NotificationRecipientIds
        );

        await _service.UpdateTicketAsync(command, cancellationToken);
        return Unit.Value;
    }
}
