using FluentAssertions;
using Moq;
using WebApi.Application.Commands.Projects.DeleteProject;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common;

namespace WebApi.Application.Tests.Commands.Projects;

public class DeleteProjectHandlerTests : BaseApplicationTest
{
    private readonly DeleteProjectHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;

    public DeleteProjectHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _projectBuilder = new ProjectBuilder();

        _handler = new DeleteProjectHandler(
            _projectRepository.Object,
            UserContext.Object,
            Clock.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var project = _projectBuilder
            .WithOwnerId(UserContext.Object.Id)
            .WithCreatedBy(UserContext.Object.Id)
            .WithClock(Clock.Object)
            .Build();
        _projectRepository.Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        var command = new DeleteProjectCommand(project.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(MediatR.Unit.Value);
        _projectRepository.Verify(x => x.DeleteAsync(project, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Projectが存在しない場合()
    {
        // Arrange
        var command = new DeleteProjectCommand(Guid.NewGuid());
        _projectRepository.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("PROJECT_NOT_FOUND");
    }

    [Fact]
    public async Task 異常系_Handle_削除可能でない場合()
    {
        // Arrange
        var project = _projectBuilder
            .WithOwnerId(Guid.NewGuid())
            .WithClock(Clock.Object)
            .Build();
        var command = new DeleteProjectCommand(project.Id);
        _projectRepository.Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<DomainException>();
    }
}
