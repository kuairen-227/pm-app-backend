using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.Domain.Tests.Helpers;

public abstract class BaseDomainTest
{
    protected readonly FakeUserContext UserContext;
    protected readonly FakeDateTimeProvider Clock;

    protected BaseDomainTest()
    {
        UserContext = new FakeUserContext();
        Clock = new FakeDateTimeProvider();
    }
}
