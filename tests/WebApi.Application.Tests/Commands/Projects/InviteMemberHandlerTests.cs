using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Projects.InviteMember;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Projects;

public class InviteMemberHandlerTests : BaseCommandHandlerTest
{
    private InviteMemberHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly ProjectBuilder _projectBuilder;
    private readonly UserBuilder _userBuilder;

    public InviteMemberHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _userRepository = new Mock<IUserRepository>();
        _projectBuilder = new ProjectBuilder();
        _userBuilder = new UserBuilder();

        _handler = new InviteMemberHandler(
            _projectRepository.Object,
            _userRepository.Object,
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
        var project = _projectBuilder.Build();
        var user = _userBuilder.Build();

        _projectRepository.Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _userRepository.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var command = new InviteMemberCommand(project.Id, user.Id, ProjectRole.RoleType.Member);
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_Projectが存在しない場合()
    {
        // Arrange
        var user = _userBuilder.Build();
        var command = new InviteMemberCommand(Guid.NewGuid(), user.Id, ProjectRole.RoleType.Member);

        _projectRepository.Setup(x => x.GetByIdAsync(command.ProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("PROJECT_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task 異常系_Userが存在しない場合()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var command = new InviteMemberCommand(project.Id, Guid.NewGuid(), ProjectRole.RoleType.Member);

        _projectRepository.Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _userRepository.Setup(x => x.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("USER_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
