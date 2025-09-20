using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Project : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public Project(
        string name, string? description, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Name は必須です");

        Name = name;
        Description = description;
    }

    public void Rename(string newName, Guid updatedBy, IDateTimeProvider clock)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Project Name は必須です");

        Name = newName;
        UpdateAuditInfo(updatedBy, clock);
    }

    public void ChangeDescription(string? newDescription, Guid updatedBy, IDateTimeProvider clock)
    {
        Description = newDescription;
        UpdateAuditInfo(updatedBy, clock);
    }
}
