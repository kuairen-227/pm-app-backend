using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Domain.Abstractions.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Project>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task AddAsync(Project project, CancellationToken cancellationToken = default);
    Task DeleteAsync(Project project, CancellationToken cancellationToken = default);
}
