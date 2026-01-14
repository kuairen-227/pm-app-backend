using MediatR;

namespace WebApi.Application.Commands.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenCommand : IRequest<RefreshTokenDto>
{
    public Guid UserId { get; set; }

    public GenerateRefreshTokenCommand(Guid userId)
    {
        UserId = userId;
    }
}
