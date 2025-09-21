namespace WebApi.Domain.Abstractions;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}
