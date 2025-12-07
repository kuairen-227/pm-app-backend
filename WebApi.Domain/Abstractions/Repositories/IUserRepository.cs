using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteAsync(User user, CancellationToken cancellationToken = default);
}
