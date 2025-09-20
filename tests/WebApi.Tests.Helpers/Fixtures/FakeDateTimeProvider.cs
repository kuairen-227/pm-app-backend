using WebApi.Domain.Abstractions;

namespace WebApi.Tests.Helpers.Fixtures;

public class FakeDateTimeProvider : IDateTimeProvider
{
    private readonly DateTimeOffset _fixedNow = new(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    public FakeDateTimeProvider(DateTimeOffset? fixedValue = null)
        => _fixedNow = fixedValue ?? _fixedNow;
    public DateTimeOffset Now => _fixedNow;
}
