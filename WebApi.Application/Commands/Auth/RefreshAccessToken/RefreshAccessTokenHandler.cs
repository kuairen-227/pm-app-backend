using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Auth.RefreshAccessToken;

public class RefreshAccessTokenHandler : BaseCommandHandler, IRequestHandler<RefreshAccessTokenCommand, AuthResult>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;

    public RefreshAccessTokenHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IJwtService jwtService,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResult> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken)
            ?? throw new AuthenticationException("INVALID_REFRESH_TOKEN", "リフレッシュトークンが無効です。");

        var accessToken = _jwtService.GenerateAccessToken(refreshToken.UserId);

        return new AuthResult(
            UserId: refreshToken.UserId,
            AccessToken: accessToken,
            RefreshToken: refreshToken.Token
        );
    }
}
