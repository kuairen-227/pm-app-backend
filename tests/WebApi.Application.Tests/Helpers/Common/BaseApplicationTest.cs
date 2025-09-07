using Moq;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions;

namespace WebApi.Domain.Tests.Helpers.Common;

public abstract class BaseApplicationTest
{
    protected readonly Mock<IUserContext> UserContext;
    protected readonly Mock<IDateTimeProvider> Clock;

    protected BaseApplicationTest()
    {
        UserContext = TestHelpers.CreateUserContext();
        Clock = TestHelpers.CreateClock();
    }
}
