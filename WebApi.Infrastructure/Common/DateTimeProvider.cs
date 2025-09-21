using WebApi.Domain.Abstractions;

namespace WebApi.Infrastructure.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}
