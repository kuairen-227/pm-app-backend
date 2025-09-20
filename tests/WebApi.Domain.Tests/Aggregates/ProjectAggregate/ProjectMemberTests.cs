using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectMemberTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Act
        var result = ProjectMember.Create(userId, role);

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
        Action act = () => ProjectMember.Create(userId, role);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("USER_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Act
        var result1 = ProjectMember.Create(userId, role);
        var result2 = ProjectMember.Create(userId, role);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role1 = ProjectRole.Create(ProjectRole.RoleType.ProjectManager);
        var role2 = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Act
        var result1 = ProjectMember.Create(userId, role1);
        var result2 = ProjectMember.Create(userId, role2);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
