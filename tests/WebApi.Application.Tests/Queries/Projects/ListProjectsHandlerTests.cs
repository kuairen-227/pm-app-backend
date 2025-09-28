using FluentAssertions;
using Moq;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Queries.Projects.Dtos;
using WebApi.Application.Queries.Projects.ListProjects;
using WebApi.Application.Tests.Helpers.Common;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common.Authorization;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Application.Tests.Queries.Projects;

public class ListProjectsHandlerTests : BaseQueryHandlerTest
{
    private readonly ListProjectsHandler _handler;
    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<IUserRepository> _userRepository;
    private readonly Mock<IPermissionService> _permissionService;
    private readonly ProjectBuilder _projectBuilder;
    private readonly UserBuilder _userBuilder;

    public ListProjectsHandlerTests()
    {
        _projectRepository = new Mock<IProjectRepository>();
        _userRepository = new Mock<IUserRepository>();
        _permissionService = new Mock<IPermissionService>();
        _projectBuilder = new ProjectBuilder();
        _userBuilder = new UserBuilder();

        Mapper.Setup(m => m.Map<IEnumerable<ProjectDto>>(It.IsAny<IEnumerable<Project>>()))
            .Returns<IEnumerable<Project>>(projects =>
                projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedBy = p.CreatedBy,
                    CreatedAt = p.CreatedAt,
                    UpdatedBy = p.UpdatedBy,
                    UpdatedAt = p.UpdatedAt,
                }));

        _handler = new ListProjectsHandler(
            _projectRepository.Object,
            _userRepository.Object,
            UserContext.Object,
            _permissionService.Object,
            Mapper.Object
        );
    }

    [Fact]
    public async Task 正常系_Handle_Admin()
    {
        // Arrange
        var projects = new List<Project>
        {
            _projectBuilder.WithName("プロジェクト1").WithDescription("説明1").Build(),
            _projectBuilder.WithName("プロジェクト2").WithDescription("説明2").Build()
        };
        var user = _userBuilder.WithRole(SystemRole.RoleType.Admin).Build();

        _projectRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);
        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _permissionService
            .Setup(x => x.HasPermissionAsync(user, ProjectPermissions.ViewAll, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var query = new ListProjectsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        for (int i = 0; i < projects.Count; i++)
        {
            result.ElementAt(i).Id.Should().Be(projects[i].Id);
            result.ElementAt(i).Name.Should().Be(projects[i].Name);
            result.ElementAt(i).Description.Should().Be(projects[i].Description);
            result.ElementAt(i).CreatedBy.Should().Be(projects[i].CreatedBy);
            result.ElementAt(i).CreatedAt.Should().Be(projects[i].CreatedAt);
            result.ElementAt(i).UpdatedBy.Should().Be(projects[i].UpdatedBy);
            result.ElementAt(i).UpdatedAt.Should().Be(projects[i].UpdatedAt);
        }

        _projectRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _projectRepository.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task 正常系_Handle_User()
    {
        // Arrange
        var projects = new List<Project>
        {
            _projectBuilder.WithName("プロジェクト1").WithDescription("説明1").Build(),
            _projectBuilder.WithName("プロジェクト2").WithDescription("説明2").Build()
        };
        var user = _userBuilder.WithRole(SystemRole.RoleType.User).Build();

        _projectRepository
            .Setup(x => x.GetByUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync([projects[0]]);
        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _permissionService
            .Setup(x => x.HasPermissionAsync(user, ProjectPermissions.ViewAll, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var query = new ListProjectsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);

        result.ElementAt(0).Id.Should().Be(projects[0].Id);
        result.ElementAt(0).Name.Should().Be(projects[0].Name);
        result.ElementAt(0).Description.Should().Be(projects[0].Description);
        result.ElementAt(0).CreatedBy.Should().Be(projects[0].CreatedBy);
        result.ElementAt(0).CreatedAt.Should().Be(projects[0].CreatedAt);
        result.ElementAt(0).UpdatedBy.Should().Be(projects[0].UpdatedBy);
        result.ElementAt(0).UpdatedAt.Should().Be(projects[0].UpdatedAt);

        _projectRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        _projectRepository.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task 正常系_Handle_プロジェクトが1件も存在しない場合()
    {
        // Arrange
        var user = _userBuilder.WithRole(SystemRole.RoleType.Admin).Build();

        _projectRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Project>());
        _userRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _permissionService
            .Setup(x => x.HasPermissionAsync(user, ProjectPermissions.ViewAll, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var query = new ListProjectsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();

        _projectRepository.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _projectRepository.Verify(x => x.GetByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
