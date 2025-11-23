using FluentAssertions;
using Moq;
using WebApi.Application.Common;
using WebApi.Application.Queries.Projects.Dtos;
using WebApi.Application.Queries.Projects.GetProjectById;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Projects;

public class GetProjectByIdHandlerTests : BaseQueryHandlerTest
{
    private readonly GetProjectByIdHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;

    public GetProjectByIdHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _projectBuilder = new ProjectBuilder();

        Mapper.Setup(m => m.Map<ProjectDetailDto>(It.IsAny<Project>()))
            .Returns<Project>(p => new ProjectDetailDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedBy = p.AuditInfo.CreatedBy,
                CreatedAt = p.AuditInfo.CreatedAt,
                UpdatedBy = p.AuditInfo.UpdatedBy,
                UpdatedAt = p.AuditInfo.UpdatedAt,
            });

        _handler = new GetProjectByIdHandler(
            _projectRepository.Object,
            Mapper.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle()
    {
        // Arrange
        var project = _projectBuilder.Build();
        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var query = new GetProjectByIdQuery(project.Id);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(project.Id);
        result.Name.Should().Be(project.Name);
        result.Description.Should().Be(project.Description);
        result.CreatedBy.Should().Be(project.AuditInfo.CreatedBy);
        result.CreatedAt.Should().Be(project.AuditInfo.CreatedAt);
        result.UpdatedBy.Should().Be(project.AuditInfo.UpdatedBy);
        result.UpdatedAt.Should().Be(project.AuditInfo.UpdatedAt);

        _projectRepository.Verify(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()), Times.Once);
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
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.PROJECT_NOT_FOUND");
        _projectRepository.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
