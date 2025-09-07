namespace WebApi.Domain.Tests.Helpers.Common;

public abstract class BaseTest
{
    protected readonly FakeDateTimeProvider Clock;

    protected BaseTest()
    {
        Clock = new FakeDateTimeProvider();
    }
}
