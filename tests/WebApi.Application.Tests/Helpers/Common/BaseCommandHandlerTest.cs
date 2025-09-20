using Moq;
using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;

namespace WebApi.Application.Tests.Helpers.Common;

public abstract class BaseCommandHandlerTest
{
    protected readonly Mock<IUnitOfWork> UnitOfWork;
    protected readonly Mock<IUserContext> UserContext;
    protected readonly Mock<IDateTimeProvider> Clock;

    protected BaseCommandHandlerTest()
    {
        UnitOfWork = TestHelpers.CreateUnitOfWork();
        UserContext = TestHelpers.CreateUserContext();
        Clock = TestHelpers.CreateClock();
    }
}
