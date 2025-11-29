using Mapster;
using WebApi.Application.Commands.Tickets.AddComment;
using WebApi.Application.Commands.Tickets.CreateTicket;
using WebApi.Application.Commands.Tickets.EditComment;
using WebApi.Application.Commands.Tickets.UpdateTicket;

namespace WebApi.Api.Dtos.Tickets;

/// <summary>
/// Ticket Mapping（DTO → Command）
/// </summary>
public class ApiMappingConfig : IRegister
{
    /// <summary>
    /// DTO → Command の Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid projectId, CreateTicketRequest), CreateTicketCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId);

        config.NewConfig<(Guid projectId, Guid ticketId, UpdateTicketRequest), UpdateTicketCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId);

        config.NewConfig<(Guid projectId, Guid ticketId, AddTicketCommentRequest), AddCommentCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId);

        config.NewConfig<(Guid projectId, Guid ticketId, Guid commentId, EditTicketCommentRequest), EditCommentCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.TicketId, src => src.ticketId)
            .Map(dest => dest.CommentId, src => src.commentId);
    }
}
