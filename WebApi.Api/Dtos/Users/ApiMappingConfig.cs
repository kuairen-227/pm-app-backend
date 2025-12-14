using Mapster;
using WebApi.Application.Commands.Users.RegisterUser;
using WebApi.Application.Commands.Users.UpdateUser;
using WebApi.Application.Common.Mapper;

namespace WebApi.Api.Dtos.Users;

/// <summary>
/// User Mapping（DTO → Command）
/// </summary>
public class ApiMappingConfig : IRegister
{
    /// <summary>
    /// DTO → Command の Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserRequest, RegisterUserCommand>()
            .Map(dest => dest.Role, src => SystemRoleMapper.Map(src.Role));
        config.NewConfig<UpdateUserRequest, UpdateUserCommand>()
            .Map(dest => dest.Role, src => SystemRoleMapper.Map(src.Role));
    }
}
