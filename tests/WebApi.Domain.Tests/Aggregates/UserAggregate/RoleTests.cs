using FluentAssertions;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class RoleTests
{
    [Theory]
    [InlineData(Role.RoleType.Admin)]
    [InlineData(Role.RoleType.Manager)]
    [InlineData(Role.RoleType.Member)]
    public void 正常系_インスタンス生成(Role.RoleType roleType)
    {
        // Act
        var role = Role.Create(roleType);

        // Assert
        role.Value.Should().Be(roleType);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var role1 = Role.Create(Role.RoleType.Admin);
        var role2 = Role.Create(Role.RoleType.Admin);

        // Assert
        role1.Should().Be(role2);
        role1.GetHashCode().Should().Be(role2.GetHashCode());
        role1.Equals(role2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var role1 = Role.Create(Role.RoleType.Admin);
        var role2 = Role.Create(Role.RoleType.Manager);

        // Assert
        role1.Should().NotBe(role2);
        role1.GetHashCode().Should().NotBe(role2.GetHashCode());
        role1.Equals(role2).Should().BeFalse();
    }
}
