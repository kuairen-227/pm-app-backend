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
            .ConstructUsing(src => new RegisterUserCommand(
                src.Name,
                src.Email,
                src.Password,
                SystemRoleMapper.Map(src.Role)
            ));

        config.NewConfig<(Guid userId, UpdateUserRequest request), UpdateUserCommand>()
            .ConstructUsing(src => new UpdateUserCommand(
                src.userId,
                src.request.Name,
                src.request.Email,
                src.request.Password,
                string.IsNullOrEmpty(src.request.Role) ? null : SystemRoleMapper.Map(src.request.Role)
            ));

        // Application DTO → Response DTO
        config.NewConfig<UserDto, UserResponse>();
    }
}
