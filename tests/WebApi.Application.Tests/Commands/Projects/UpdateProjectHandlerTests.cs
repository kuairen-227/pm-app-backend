using FluentAssertions;
using MediatR;
using Moq;
using WebApi.Application.Abstractions;
using WebApi.Application.Commands.Projects.UpdateProject;
using WebApi.Application.Common;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Commands.Projects;

public class UpdateProjectHandlerTests : BaseCommandHandlerTest
{
    private readonly UpdateProjectHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly ProjectBuilder _projectBuilder;

    public UpdateProjectHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _projectBuilder = new ProjectBuilder();

        _handler = new UpdateProjectHandler(
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
        _projectRepository
            .Setup(x => x.GetByIdAsync(project.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var command = new UpdateProjectCommand(
            project.Id,
            "プロジェクト名 - 編集",
            "プロジェクト説明 - 編集"
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
        project.Name.Should().Be(command.Name);
        project.Description.Should().Be(command.Description);
        project.AuditInfo.UpdatedBy.Should().Be(UserContext.Object.Id);
        project.AuditInfo.UpdatedAt.Should().Be(Clock.Now);

        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task 異常系_Handle_Projectが存在しない場合()
    {
        // Arrange
        var command = new UpdateProjectCommand(
            Guid.NewGuid(),
            "プロジェクト名 - 編集",
            "プロジェクト説明 - 編集"
        );
        _projectRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        var ex = await act.Should().ThrowAsync<NotFoundException>();
        ex.Which.ErrorCode.Should().Be("APPLICATION.PROJECT_NOT_FOUND");
        UnitOfWork.Verify(x => x.SaveChangesAsync(
            It.IsAny<IDomainEventPublisher>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
