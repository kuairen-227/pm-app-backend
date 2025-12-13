using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Commands.Auth.RevokeRefreshToken;

public class RevokeRefreshTokenHandler : BaseCommandHandler, IRequestHandler<RevokeRefreshTokenCommand, Unit>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RevokeRefreshTokenHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IDomainEventPublisher domainEventPublisher,
        IUserContext userContext,
        IDateTimeProvider clock
    ) : base(unitOfWork, domainEventPublisher, userContext, clock)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken)
            ?? throw new AuthenticationException("INVALID_REFRESH_TOKEN", "リフレッシュトークンが無効です。");

        refreshToken.Revoke(UserContext.Id, Clock);

        await UnitOfWork.SaveChangesAsync(DomainEventPublisher, cancellationToken);
        return Unit.Value;
    }
}
