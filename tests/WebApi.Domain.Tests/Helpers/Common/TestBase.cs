namespace WebApi.Domain.Tests.Helpers.Common;

public abstract class TestBase
{
    protected readonly FakeDateTimeProvider Clock;

    protected TestBase()
    {
        Clock = new FakeDateTimeProvider();
    }
}
