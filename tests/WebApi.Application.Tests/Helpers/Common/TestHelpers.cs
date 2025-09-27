using AutoMapper;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;

namespace WebApi.Application.Tests.Helpers.Common;

public static class TestHelpers
{
    public static Mock<IMapper> CreateMapper()
    {
        var mock = new Mock<IMapper>();
        return mock;
    }

    public static Mock<IUnitOfWork> CreateUnitOfWork()
    {
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return mock;
    }

    public static Mock<IDomainEventPublisher> CreateDomainEventPublisher()
    {
        var mock = new Mock<IDomainEventPublisher>();
        mock.Setup(x => x.PublishAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return mock;
    }

    public static Mock<IUserContext> CreateUserContext(Guid? userId = null)
    {
        var mock = new Mock<IUserContext>();
        mock.SetupGet(x => x.Id).Returns(userId ?? Guid.NewGuid());
        return mock;
    }

    public static Mock<IDateTimeProvider> CreateClock(DateTime? now = null)
    {
        var mock = new Mock<IDateTimeProvider>();
        mock.Setup(x => x.Now).Returns(now ?? DateTime.UtcNow);
        mock.Setup(x => x.Today).Returns(
            now.HasValue ? DateOnly.FromDateTime(now.Value) : DateOnly.FromDateTime(DateTime.UtcNow));
        return mock;
    }
}
