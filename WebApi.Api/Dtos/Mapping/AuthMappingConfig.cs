using Mapster;
using WebApi.Api.Dtos.Response.Auth;
using WebApi.Application.Queries.Users.Dtos;

namespace WebApi.Api.Dtos.Mapping;

/// <summary>
/// Auth Mapping
/// </summary>
public class AuthMappingConfig : IRegister
{
    /// <summary>
    /// Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        // Application DTO → Response DTO
        config.NewConfig<UserDto, MeResponse>();
    }
}
