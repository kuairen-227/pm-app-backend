namespace WebApi.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}
