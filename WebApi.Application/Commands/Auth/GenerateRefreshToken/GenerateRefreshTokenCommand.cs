using MediatR;

namespace WebApi.Application.Commands.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenCommand : IRequest<RefreshTokenDto>
{
    public GenerateRefreshTokenCommand()
    {
    }
}
