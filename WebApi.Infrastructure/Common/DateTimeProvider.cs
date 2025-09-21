using WebApi.Domain.Abstractions;

namespace WebApi.Infrastructure.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
    public DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);
}
