using FluentAssertions;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Projects.LaunchProject;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Projects;

public class LaunchProjectHandlerTests : BaseCommandHandlerTest
{
    private readonly LaunchProjectHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;

    public LaunchProjectHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _projectBuilder = new ProjectBuilder();

        _handler = new LaunchProjectHandler(
            _projectRepository.Object,
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
        Project? capturedProject = null;
        _projectRepository
            .Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .Callback<Project, CancellationToken>((p, _) => capturedProject = p)
            .Returns(Task.CompletedTask);

        // Act
        var command = new LaunchProjectCommand(
            project.Name,
            project.Description
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        capturedProject.Should().NotBeNull();
        capturedProject.Name.Should().Be(project.Name);
        capturedProject.Description.Should().Be(project.Description);
        capturedProject.AuditInfo.CreatedBy.Should().Be(UserContext.Object.Id);
        capturedProject.AuditInfo.CreatedAt.Should().Be(Clock.Now);
        capturedProject.AuditInfo.UpdatedBy.Should().Be(UserContext.Object.Id);
        capturedProject.AuditInfo.UpdatedAt.Should().Be(Clock.Now);

        _projectRepository.Verify(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
