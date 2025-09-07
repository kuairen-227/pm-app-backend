using FluentAssertions;
using Moq;
using WebApi.Application.Queries.Projects.GetProjectById;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Tests.Helpers;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Application.Tests.Queries.Projects;

public class GetProjectByIdHandlerTests : BaseApplicationTest
{
    private readonly GetProjectByIdHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;

    public GetProjectByIdHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _projectBuilder = new ProjectBuilder();

        _handler = new GetProjectByIdHandler(_projectRepository.Object);
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var query = new GetProjectByIdQuery(project.Id);
        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(project.Id);
        result.Name.Should().Be(project.Name);
        result.Description.Should().Be(project.Description);
        result.OwnerId.Should().Be(project.OwnerId);
        result.CreatedBy.Should().Be(project.CreatedBy);
        result.CreatedAt.Should().Be(project.CreatedAt);
        result.UpdatedBy.Should().Be(project.UpdatedBy);
        result.UpdatedAt.Should().Be(project.UpdatedAt);
        _projectRepository.Verify(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task 異常系_Handle_Projectが存在しない場合()
    {
        // Arrange
        var query = new GetProjectByIdQuery(Guid.NewGuid());
        _projectRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _projectRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
    }
}
