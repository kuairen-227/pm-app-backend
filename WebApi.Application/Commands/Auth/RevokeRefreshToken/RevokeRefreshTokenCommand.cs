using MediatR;

namespace WebApi.Application.Commands.Auth.RevokeRefreshToken;

public class RevokeRefreshTokenCommand : IRequest<Unit>
{
    public string RefreshToken { get; set; } = null!;

    public RevokeRefreshTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
