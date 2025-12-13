using MediatR;
using WebApi.Application.Abstractions.AuthService;

namespace WebApi.Application.Commands.Auth.RefreshAccessToken;

public class RefreshAccessTokenCommand : IRequest<AuthResult>
{
    public string RefreshToken { get; set; } = null!;

    public RefreshAccessTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
