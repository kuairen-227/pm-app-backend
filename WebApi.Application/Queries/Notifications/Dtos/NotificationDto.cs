namespace WebApi.Application.Queries.Notifications.Dtos;

public sealed class NotificationDto
{
    public Guid Id { get; init; }
    public string Category { get; init; } = string.Empty;
    public Guid RecipientId { get; init; }
    public Guid SubjectId { get; init; }
    public string Message { get; init; } = string.Empty;
    public bool IsRead { get; init; }
    public Guid CreatedBy { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid UpdatedBy { get; init; }
    public DateTime UpdatedAt { get; init; }
}
