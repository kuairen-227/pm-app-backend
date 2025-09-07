using FluentAssertions;
using Moq;
using WebApi.Application.Queries.Projects.ListProjects;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Tests.Helpers;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Application.Tests.Queries.Projects;

public class ListProjectsHandlerTests : BaseApplicationTest
{
    private readonly ListProjectsHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;

    public ListProjectsHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _projectBuilder = new ProjectBuilder();

        _handler = new ListProjectsHandler(_projectRepository.Object);
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var projects = new List<Project>
        {
            _projectBuilder.WithName("プロジェクト1").WithDescription("説明1").WithOwnerId(Guid.NewGuid()).Build(),
            _projectBuilder.WithName("プロジェクト2").WithDescription("説明2").WithOwnerId(Guid.NewGuid()).Build()
        };
        _projectRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);
        var query = new ListProjectsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        result.ElementAt(0).Id.Should().Be(projects[0].Id);
        result.ElementAt(0).Name.Should().Be(projects[0].Name);
        result.ElementAt(0).Description.Should().Be(projects[0].Description);
        result.ElementAt(0).OwnerId.Should().Be(projects[0].OwnerId);
        result.ElementAt(0).CreatedBy.Should().Be(projects[0].CreatedBy);
        result.ElementAt(0).CreatedAt.Should().Be(projects[0].CreatedAt);
        result.ElementAt(0).UpdatedBy.Should().Be(projects[0].UpdatedBy);
        result.ElementAt(0).UpdatedAt.Should().Be(projects[0].UpdatedAt);

        result.ElementAt(1).Id.Should().Be(projects[1].Id);
        result.ElementAt(1).Name.Should().Be(projects[1].Name);
        result.ElementAt(1).Description.Should().Be(projects[1].Description);
        result.ElementAt(1).OwnerId.Should().Be(projects[1].OwnerId);
        result.ElementAt(1).CreatedBy.Should().Be(projects[1].CreatedBy);
        result.ElementAt(1).CreatedAt.Should().Be(projects[1].CreatedAt);
        result.ElementAt(1).UpdatedBy.Should().Be(projects[1].UpdatedBy);
        result.ElementAt(1).UpdatedAt.Should().Be(projects[1].UpdatedAt);

        _projectRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 正常系_Handle_プロジェクトが1件も存在しない場合()
    {
        // Arrange
        _projectRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Project>());
        var query = new ListProjectsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
        _projectRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
