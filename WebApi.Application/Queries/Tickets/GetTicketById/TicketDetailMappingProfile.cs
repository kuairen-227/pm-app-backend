using AutoMapper;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.GetTicketById;

public class TicketDetailMappingProfile : Profile
{
    public TicketDetailMappingProfile()
    {
        CreateMap<Ticket, TicketDetailDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value.ToString()))
            .ForMember(dest => dest.Deadline,
                opt => opt.MapFrom(src => src.Deadline != null ? src.Deadline.Value : (DateTimeOffset?)null))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
            .ForMember(dest => dest.AssignmentHistories, opt => opt.MapFrom(src => src.AssignmentHistories));

        CreateMap<TicketComment, TicketCommentDto>();

        CreateMap<AssignmentHistory, AssignmentHistoryDto>()
            .ForMember(dest => dest.ChangeType, opt => opt.MapFrom(src => src.ChangeType.ToString()));
    }
}
