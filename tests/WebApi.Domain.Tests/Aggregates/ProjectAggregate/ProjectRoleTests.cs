using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectRoleTests
{
    [Theory]
    [InlineData(ProjectRole.RoleType.ProjectManager)]
    [InlineData(ProjectRole.RoleType.Member)]
    public void 正常系_インスタンス生成(ProjectRole.RoleType roleType)
    {
        // Act
        var result = ProjectRole.Create(roleType);

        // Assert
        result.Value.Should().Be(roleType);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = ProjectRole.Create(ProjectRole.RoleType.Member);
        var result2 = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var result1 = ProjectRole.Create(ProjectRole.RoleType.ProjectManager);
        var result2 = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
