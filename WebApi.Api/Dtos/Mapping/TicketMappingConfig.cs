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
        config.NewConfig<(Guid projectId, CreateTicketRequest), CreateTicketCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId);

        config.NewConfig<(Guid projectId, Guid ticketId, UpdateTicketRequest request), UpdateTicketCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId)
            .Map(dest => dest.Title, src => src.request.Title.ToOptional())
            .Map(dest => dest.Description, src => src.request.Description.ToOptional())
            .Map(dest => dest.AssigneeId, src => src.request.AssigneeId.ToOptional())
            .Map(dest => dest.StartDate, src => src.request.StartDate.ToOptional())
            .Map(dest => dest.EndDate, src => src.request.EndDate.ToOptional())
            .Map(dest => dest.Status, src => src.request.Status.ToOptional())
            .Map(dest => dest.CompletionCriterionOperations, src => src.request.CompletionCriterionOperations.ToOptional())
            .Map(dest => dest.Comment, src => src.request.Comment.ToOptional());

        config.NewConfig<(Guid projectId, Guid ticketId, AddCompletionCriterionRequest), AddCompletionCriterionCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId);

        config.NewConfig<(Guid projectId, Guid ticketId, Guid criterionId, EditCompletionCriterionRequest), EditCompletionCriterionCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId)
            .Map(dest => dest.CriterionId, src => src.criterionId);

        config.NewConfig<(Guid projectId, Guid ticketId, AddTicketCommentRequest request), AddCommentCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId)
            .Map(dest => dest.AssigneeId, src => src.request.AssigneeId.ToOptional())
            .Map(dest => dest.StartDate, src => src.request.StartDate.ToOptional())
            .Map(dest => dest.EndDate, src => src.request.EndDate.ToOptional())
            .Map(dest => dest.Status, src => src.request.Status.ToOptional())
            .Map(dest => dest.Comment, src => src.request.Comment.ToOptional());

        config.NewConfig<(Guid projectId, Guid ticketId, Guid commentId, EditTicketCommentRequest), EditCommentCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId)
            .Map(dest => dest.CommentId, src => src.commentId);

        // Application DTO → Response DTO
        config.NewConfig<TicketDto, TicketResponse>();

        config.NewConfig<TicketDetailDto, TicketDetailResponse>();

        config.NewConfig<TicketCompletionCriterionDto, TicketCompletionCriterionResponse>();

        config.NewConfig<TicketCommentDto, TicketCommentResponse>();

        config.NewConfig<TicketHistoryDto, TicketHistoryResponse>();

        config.NewConfig<TicketHistoryChangeDto, TicketHistoryChangeResponse>();
    }
}
