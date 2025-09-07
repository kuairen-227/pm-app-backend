namespace WebApi.Domain.Tests.Helpers.Common;

public abstract class BaseDomainTest
{
    protected readonly FakeDateTimeProvider Clock;

    protected BaseDomainTest()
    {
        Clock = new FakeDateTimeProvider();
    }
}
