using Moq;
using WebApi.Application.Abstractions;
using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.Application.Tests.Helpers.Common;

public abstract class BaseServiceHandlerTest
{
    protected readonly Mock<IUnitOfWork> UnitOfWork;
    protected readonly Mock<IDomainEventPublisher> DomainEventPublisher;
    protected readonly Mock<IUserContext> UserContext;
    protected readonly FakeDateTimeProvider Clock;

    protected BaseServiceHandlerTest()
    {
        UnitOfWork = TestHelpers.CreateUnitOfWork();
        DomainEventPublisher = TestHelpers.CreateDomainEventPublisher();
        UserContext = TestHelpers.CreateUserContext();
        Clock = new FakeDateTimeProvider();
    }
}
