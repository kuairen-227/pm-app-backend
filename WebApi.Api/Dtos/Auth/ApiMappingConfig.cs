using Mapster;
using WebApi.Application.Commands.Auth.RefreshAccessToken;
using WebApi.Application.Commands.Auth.RevokeRefreshToken;

namespace WebApi.Api.Dtos.Auth;

/// <summary>
/// Auth Mapping（DTO → Command）
/// </summary>
public class ApiMappingConfig : IRegister
{
    /// <summary>
    /// DTO → Command の Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RefreshRequest, RefreshAccessTokenCommand>();
        config.NewConfig<LogoutRequest, RevokeRefreshTokenCommand>();
    }
}
