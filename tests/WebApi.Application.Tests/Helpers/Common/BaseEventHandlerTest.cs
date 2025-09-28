using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Abstractions.Repositories;

namespace WebApi.Application.Tests.Helpers.Common;

public abstract class BaseEventHandlerTest
{
    protected readonly Mock<INotificationRepository> NotificationRepository;
    protected readonly Mock<IUnitOfWork> UnitOfWork;
    protected readonly Mock<IUserContext> UserContext;
    protected readonly Mock<IDateTimeProvider> Clock;

    protected BaseEventHandlerTest()
    {
        NotificationRepository = new Mock<INotificationRepository>();
        UnitOfWork = TestHelpers.CreateUnitOfWork();
        UserContext = TestHelpers.CreateUserContext();
        Clock = TestHelpers.CreateClock();
    }
}
