using Mapster;
using WebApi.Api.Dtos.Request.Tickets;
using WebApi.Api.Dtos.Response.Tickets;
using WebApi.Api.Mapping;
using WebApi.Application.Commands.Tickets.AddComment;
using WebApi.Application.Commands.Tickets.AddCompletionCriterion;
using WebApi.Application.Commands.Tickets.CreateTicket;
using WebApi.Application.Commands.Tickets.EditComment;
using WebApi.Application.Commands.Tickets.EditCompletionCriterion;
using WebApi.Application.Commands.Tickets.UpdateTicket;
using WebApi.Application.Queries.Tickets.Dtos;

namespace WebApi.Api.Dtos.Mapping;

/// <summary>
/// Ticket Mapping
/// </summary>
public class ApiMappingConfig : IRegister
{
    /// <summary>
    /// Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        // Request DTO → Command
        config.NewConfig<(Guid projectId, CreateTicketRequest request), CreateTicketCommand>()
            .ConstructUsing(src => new CreateTicketCommand(
                src.projectId,
                src.request.Title,
                src.request.Description,
                src.request.AssigneeId,
                src.request.StartDate,
                src.request.EndDate,
                src.request.CompletionCriteria,
                src.request.NotificationRecipientIds
            ));

        config.NewConfig<(Guid projectId, Guid ticketId, UpdateTicketRequest request), UpdateTicketCommand>()
            .ConstructUsing(src => new UpdateTicketCommand(
                src.projectId,
                src.ticketId,
                src.request.Title.ToOptional(),
                src.request.Description.ToOptional(),
                src.request.AssigneeId.ToOptional(),
                src.request.StartDate.ToOptional(),
                src.request.EndDate.ToOptional(),
                src.request.Status.ToOptional(),
                src.request.CompletionCriterionOperations.ToOptional(),
                src.request.Comment.ToOptional(),
                src.request.NotificationRecipientIds
            ));

        config.NewConfig<(Guid projectId, Guid ticketId, AddCompletionCriterionRequest request), AddCompletionCriterionCommand>()
            .ConstructUsing(src => new AddCompletionCriterionCommand(
                src.projectId,
                src.ticketId,
                src.request.Criterion
            ));

        config.NewConfig<(Guid projectId, Guid ticketId, Guid criterionId, EditCompletionCriterionRequest request), EditCompletionCriterionCommand>()
            .ConstructUsing(src => new EditCompletionCriterionCommand(
                src.projectId,
                src.ticketId,
                src.criterionId,
                src.request.Criterion
            ));

        config.NewConfig<(Guid projectId, Guid ticketId, AddTicketCommentRequest request), AddCommentCommand>()
            .ConstructUsing(src => new AddCommentCommand(
                src.projectId,
                src.ticketId,
                src.request.AssigneeId.ToOptional(),
                src.request.StartDate.ToOptional(),
                src.request.EndDate.ToOptional(),
                src.request.Status.ToOptional(),
                src.request.Comment.ToOptional(),
                src.request.NotificationRecipientIds
            ));

        config.NewConfig<(Guid projectId, Guid ticketId, Guid commentId, EditTicketCommentRequest request), EditCommentCommand>()
            .ConstructUsing(src => new EditCommentCommand(
                src.projectId,
                src.ticketId,
                src.commentId,
                src.request.Content
            ));

        // Application DTO → Response DTO
        config.NewConfig<TicketDto, TicketResponse>();

        config.NewConfig<TicketDetailDto, TicketDetailResponse>();

        config.NewConfig<TicketCompletionCriterionDto, TicketCompletionCriterionResponse>();

        config.NewConfig<TicketCommentDto, TicketCommentResponse>();

        config.NewConfig<TicketHistoryDto, TicketHistoryResponse>();

        config.NewConfig<TicketHistoryChangeDto, TicketHistoryChangeResponse>();
    }
}
