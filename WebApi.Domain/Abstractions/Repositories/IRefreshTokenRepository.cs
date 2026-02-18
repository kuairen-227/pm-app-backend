using WebApi.Domain.Aggregates.AuthAggregate;

namespace WebApi.Domain.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(
        string token, IDateTimeProvider clock, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
}
