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
using WebApi.Application.Queries.Tickets.ListProjectTickets;

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
        // Request DTO → Query
        config.NewConfig<(Guid projectId, ListProjectTicketsRequest request), ListProjectTicketsQuery>()
            .ConstructUsing(src => new ListProjectTicketsQuery(src.projectId))
            .Map(dest => dest.Pagination.PageNumber, src => src.request.PageNumber)
            .Map(dest => dest.Pagination.PageSize, src => src.request.PageSize)
            .Map(dest => dest.Sorting.SortBy, src => src.request.SortBy)
            .Map(dest => dest.Sorting.SortOrder, src => src.request.SortOrder)
            .Map(dest => dest.Filter.Title, src => src.request.Title)
            .Map(dest => dest.Filter.AssigneeId, src => src.request.AssigneeId)
            .Map(dest => dest.Filter.Status, src => src.request.Status)
            .Map(dest => dest.Filter.StartDateFrom, src => src.request.StartDateFrom)
            .Map(dest => dest.Filter.StartDateTo, src => src.request.StartDateTo)
            .Map(dest => dest.Filter.EndDateFrom, src => src.request.EndDateFrom)
            .Map(dest => dest.Filter.EndDateTo, src => src.request.EndDateTo);

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
