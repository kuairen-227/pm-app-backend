using FluentAssertions;
using Moq;
using WebApi.Application.Common;
using WebApi.Application.Events.Projects.MemberInvited;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Services.NotificationFactories;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Projects;

public class MemberInvitedHandlerTests : BaseEventHandlerTest
{
    private MemberInvitedHandler _handler;
    private readonly ProjectNotificationFactory _notificationFactory;
    private readonly Mock<INotificationRepository> _notificationRepository;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;
    private readonly UserBuilder _userBuilder;

    public MemberInvitedHandlerTests()
    {
        _notificationRepository = new Mock<INotificationRepository>();
        _projectRepository = new Mock<IProjectRepository>();
        _notificationFactory = new ProjectNotificationFactory(Clock);
        _projectBuilder = new ProjectBuilder();
        _userBuilder = new UserBuilder();

        _handler = new MemberInvitedHandler(
            _notificationFactory,
            _notificationRepository.Object,
            _projectRepository.Object,
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

        _projectRepository.Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var notification = new MemberInvitedNotification(project.Id, user.Id, ProjectRole.RoleType.Member);
        await _handler.Handle(notification, CancellationToken.None);

        // Assert
        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Domain.Aggregates.NotificationAggregate.Notification>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);
    }

    [Fact]
    public async Task 異常系_Projectが存在しない場合()
    {
        // Arrange
        _projectRepository.Setup(x => x.GetByIdAsync(Guid.NewGuid(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var notification = new MemberInvitedNotification(Guid.NewGuid(), Guid.NewGuid(), ProjectRole.RoleType.Member);
        var act = async () => await _handler.Handle(notification, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("PROJECT_NOT_FOUND");
        _notificationRepository.Verify(x => x.AddAsync(
            It.IsAny<Domain.Aggregates.NotificationAggregate.Notification>(),
            It.IsAny<CancellationToken>()
        ), Times.Never);
    }
}
