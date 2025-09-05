namespace WebApi.Domain.Abstractions;

public interface IDateTimeProvider
{
    DateTimeOffset Now { get; }
}
