using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Notifications.MarkNotificationAsRead;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Notifications;

public class MarkNotificationAsReadHandlerTests : BaseCommandHandlerTest
{
    private MarkNotificationAsReadHandler _handler;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly NotificationBuilder _notificationBuilder;

    public MarkNotificationAsReadHandlerTests()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationBuilder = new NotificationBuilder();

        _handler = new MarkNotificationAsReadHandler(
            _notificationRepository.Object,
            UnitOfWork.Object,
            DomainEventPublisher.Object,
            UserContext.Object,
            Clock
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var notification = _notificationBuilder
            .WithRecipientId(UserContext.Object.Id).Build();

        _notificationRepository
            .Setup(x => x.GetByIdAsync(notification.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(notification);

        // Act
        var command = new MarkNotificationAsReadCommand(notification.Id);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Notificationが存在しない場合()
    {
        // Arrange
        var notification = _notificationBuilder.Build();

        _notificationRepository
            .Setup(x => x.GetByIdAsync(notification.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Notification?)null);

        // Act
        var command = new MarkNotificationAsReadCommand(notification.Id);
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.NOTIFICATION_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
