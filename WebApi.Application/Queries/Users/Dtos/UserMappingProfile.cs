using AutoMapper;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Queries.Users.Dtos;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}
