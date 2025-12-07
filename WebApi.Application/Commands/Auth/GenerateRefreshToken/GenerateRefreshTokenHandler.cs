using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.AuthAggregate;

namespace WebApi.Application.Commands.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenHandler : BaseCommandHandler, IRequestHandler<GenerateRefreshTokenCommand, RefreshTokenDto>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public GenerateRefreshTokenHandler(
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<RefreshTokenDto> Handle(GenerateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = _tokenService.GenerateSecureToken();

        var refreshToken = new RefreshToken(
            userId: request.UserId,
            token: token,
            expiresAt: Clock.Now.AddDays(7),
            createdBy: UserContext.Id,
            clock: Clock
        );

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);

        return new RefreshTokenDto { RefreshToken = refreshToken.Token };
    }
}
