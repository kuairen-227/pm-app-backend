using WebApi.Domain.Abstractions;

namespace WebApi.Tests.Helpers.Fixtures;

public class FakeDateTimeProvider : IDateTimeProvider
{
    private readonly DateTime _fixedNow = new(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
    public FakeDateTimeProvider(DateTime? fixedValue = null)
        => _fixedNow = fixedValue ?? _fixedNow;
    public DateTime Now => _fixedNow;
}
