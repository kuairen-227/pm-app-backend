using AutoMapper;
using WebApi.Domain.Aggregates.NotificationAggregate;

namespace WebApi.Application.Queries.Notifications.Dtos;

public class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<Notification, NotificationDto>();
    }
}
