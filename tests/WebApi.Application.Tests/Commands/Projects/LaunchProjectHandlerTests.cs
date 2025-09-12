using FluentAssertions;
using Moq;
using WebApi.Application.Commands.Projects.LaunchProject;
using WebApi.Application.Tests.Helpers;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Tests.Commands.Projects;

public class CreateProjectHandlerTests : BaseApplicationTest
{
    private readonly LaunchProjectHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;

    public CreateProjectHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _projectBuilder = new ProjectBuilder();

        _handler = new LaunchProjectHandler(
            _projectRepository.Object,
            UserContext.Object,
            Clock.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var command = new LaunchProjectCommand(
            project.Name,
            project.Description,
            project.OwnerId
        );
        Project? capturedProject = null;
        _projectRepository
            .Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Callback<Project, CancellationToken>((p, _) => capturedProject = p)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        capturedProject.Should().NotBeNull();
        capturedProject.Name.Should().Be(project.Name);
        capturedProject.Description.Should().Be(project.Description);
        capturedProject.OwnerId.Should().Be(project.OwnerId);
        capturedProject.CreatedBy.Should().Be(UserContext.Object.Id);
        capturedProject.CreatedAt.Should().Be(Clock.Object.Now);
        capturedProject.UpdatedBy.Should().Be(UserContext.Object.Id);
        capturedProject.UpdatedAt.Should().Be(Clock.Object.Now);
        _projectRepository.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
