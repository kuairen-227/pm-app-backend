namespace WebApi.Application.Queries.Tickets;

public class TicketCommentDto
{
    public Guid AuthorId { get; set; }
    public string Content { get; set; } = null!;
}
