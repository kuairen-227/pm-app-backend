using AutoMapper;
using WebApi.Domain.Aggregates.TicketAggregate;

namespace WebApi.Application.Queries.Tickets.Dtos;

public class TicketDetailMappingProfile : Profile
{
    public TicketDetailMappingProfile()
    {
        CreateMap<Ticket, TicketBaseDto>()
            .Include<Ticket, TicketDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Value.ToString()))
            .ForMember(dest => dest.Deadline,
                opt => opt.MapFrom(src => src.Deadline != null ? src.Deadline.Value : (DateTime?)null));

        CreateMap<TicketComment, TicketCommentDto>();

        CreateMap<AssignmentHistory, AssignmentHistoryDto>()
            .ForMember(dest => dest.ChangeType, opt => opt.MapFrom(src => src.ChangeType.ToString()));
    }
}
