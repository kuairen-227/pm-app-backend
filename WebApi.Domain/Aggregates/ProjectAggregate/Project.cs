using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Project : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }

    public Project(
        string name, string? description, Guid ownerId, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Name は必須です");
        if (ownerId == Guid.Empty)
            throw new DomainException("PROJECT_OWNER_ID_REQUIRED", "OwnerId は必須です");

        Name = name;
        Description = description;
        OwnerId = ownerId;
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

    public void ChangeOwner(Guid newOwnerId, Guid updatedBy, IDateTimeProvider clock)
    {
        if (newOwnerId == Guid.Empty)
            throw new DomainException("PROJECT_OWNER_ID_REQUIRED", "Project OwnerId は必須です");

        OwnerId = newOwnerId;
        UpdateAuditInfo(updatedBy, clock);
    }

    public Ticket CreateTicket(
        TicketTitle title, Guid? assigneeId, Deadline? deadline, string? completionCriteria, Guid createdBy, IDateTimeProvider clock)
    {
        return new Ticket(Id, title, assigneeId, deadline, completionCriteria, createdBy, clock);
    }

    public void EnsureDeletable(Guid requesterId)
    {
        if (OwnerId != requesterId)
            throw new DomainException("PROJECT_NOT_DELETABLE", "このプロジェクトは削除できません");
    }
}
