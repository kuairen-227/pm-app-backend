using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectMemberTests : BaseDomainTest
{
    private readonly ProjectMemberBuilder _projectMemberBuilder;

    public ProjectMemberTests()
    {
        _projectMemberBuilder = new ProjectMemberBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Act
        var result = new ProjectMember(userId, role, Guid.NewGuid(), Clock);

        // Assert
        result.UserId.Should().Be(userId);
        result.Role.Should().Be(role);
    }

    [Fact]
    public void 異常系_インスタンス生成_UserIdが空の場合()
    {
        // Arrange
        var userId = Guid.Empty;
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Act
        var act = () => new ProjectMember(userId, role, Guid.NewGuid(), Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeRole()
    {
        // Arrange
        var projectMember = _projectMemberBuilder
            .WithRole(ProjectRole.RoleType.Member)
            .Build();
        var newRole = ProjectRole.Create(ProjectRole.RoleType.ProjectManager);
        var updatedBy = Guid.NewGuid();

        // Act
        projectMember.ChangeRole(newRole, updatedBy);

        // Assert
        projectMember.Role.Should().Be(newRole);
    }
}
