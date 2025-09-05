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

    public Ticket CreateTicket(TicketTitle title, Deadline deadline, Guid createdBy, IDateTimeProvider clock)
    {
        return new Ticket(Id, title, deadline, createdBy, clock);
    }

    public void EnsureDeletable(Guid requesterId)
    {
        if (OwnerId != requesterId)
            throw new DomainException("PROJECT_NOT_DELETABLE", "このプロジェクトは削除できません");
    }
}
