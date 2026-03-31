using AutoMapper;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Queries.Users.Dtos;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Value));
    }
}
