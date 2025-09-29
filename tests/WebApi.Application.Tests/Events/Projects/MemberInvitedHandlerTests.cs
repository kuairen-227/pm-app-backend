using FluentAssertions;
using Moq;
using WebApi.Application.Events.Projects.MemberInvited;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Events.Projects;

public class MemberInvitedHandlerTests : BaseEventHandlerTest
{
    private MemberInvitedHandler _handler;
    private readonly ProjectNotificationFactory _notificationFactory;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly ProjectBuilder _projectBuilder;
    private readonly UserBuilder _userBuilder;

    public MemberInvitedHandlerTests()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _notificationFactory = new ProjectNotificationFactory(Clock);
        _projectBuilder = new ProjectBuilder();
        _userBuilder = new UserBuilder();

        _handler = new MemberInvitedHandler(
            _notificationFactory,
            _notificationRepository.Object,
            UnitOfWork.Object,
            UserContext.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var user = _userBuilder.Build();

        // Act
        var notification = new MemberInvitedNotification(project.Id, project.Name, user.Id);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Notification>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
