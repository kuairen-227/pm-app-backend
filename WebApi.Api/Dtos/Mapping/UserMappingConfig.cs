using Mapster;
using WebApi.Api.Dtos.Request.Users;
using WebApi.Api.Dtos.Response.Users;
using WebApi.Application.Commands.Users.RegisterUser;
using WebApi.Application.Commands.Users.UpdateUser;
using WebApi.Application.Common.Mapper;
using WebApi.Application.Queries.Users.Dtos;

namespace WebApi.Api.Dtos.Mapping;

/// <summary>
/// User Mapping
/// </summary>
public class UserMappingConfig : IRegister
{
    /// <summary>
    /// Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        // Request DTO → Command
        config.NewConfig<RegisterUserRequest, RegisterUserCommand>()
            .Map(dest => dest.Role, src => SystemRoleMapper.Map(src.Role));
        config.NewConfig<UpdateUserRequest, UpdateUserCommand>()
            .Map(dest => dest.Role, src => SystemRoleMapper.Map(src.Role));

        // Application DTO → Response DTO
        config.NewConfig<UserDto, UserResponse>();
    }
}
