using MediatR;
using WebApi.Domain.Aggregates.AuthAggregate;

namespace WebApi.Application.Commands.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenCommand : IRequest<RefreshTokenDto>
{
    public GenerateRefreshTokenCommand()
    {
    }
}
