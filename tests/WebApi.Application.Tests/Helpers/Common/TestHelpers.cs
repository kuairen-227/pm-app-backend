using AutoMapper;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Fixtures;

namespace WebApi.Application.Tests.Helpers.Common;

public static class TestHelpers
{
    public static Mock<IUnitOfWork> CreateUnitOfWork()
    {
        var mock = new Mock<IUnitOfWork>();
        mock.Setup(x => x.SaveChangesAsync(
                It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()))
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
}
