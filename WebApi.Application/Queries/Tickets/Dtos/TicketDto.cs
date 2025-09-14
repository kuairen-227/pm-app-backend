namespace WebApi.Application.Queries.Tickets.Dtos;

public class TicketDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public Guid? AssigneeId { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public string Status { get; set; } = null!;
    public string? CompletionCriteria { get; set; }
}
