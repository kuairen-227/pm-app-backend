using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.Domain.Tests.Helpers;

public abstract class BaseDomainTest
{
    protected readonly FakeDateTimeProvider Clock;

    protected BaseDomainTest()
    {
        Clock = new FakeDateTimeProvider();
    }
}
