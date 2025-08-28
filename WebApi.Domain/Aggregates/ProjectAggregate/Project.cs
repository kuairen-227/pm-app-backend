using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Project : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid OwnerId { get; private set; }

    private readonly List<Ticket> _tickets = new();
    public IReadOnlyList<Ticket> Tickets => _tickets.AsReadOnly();

    public Project(string name, string? description, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Name は必須です");
        if (ownerId == Guid.Empty)
            throw new DomainException("PROJECT_OWNER_ID_REQUIRED", "OwnerId は必須です");

        Name = name;
        Description = description;
        OwnerId = ownerId;
    }

    public void AddTicket(Ticket ticket)
    {
        if (ticket is null)
            throw new DomainException("TICKET_REQUIRED", "Ticket は必須です");

        _tickets.Add(ticket);
    }
}
