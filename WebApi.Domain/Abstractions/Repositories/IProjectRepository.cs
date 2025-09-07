using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Domain.Abstractions.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Project?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task AddAsync(Project project, CancellationToken cancellationToken = default);
    Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
    Task DeleteAsync(Project project, CancellationToken cancellationToken = default);
}
