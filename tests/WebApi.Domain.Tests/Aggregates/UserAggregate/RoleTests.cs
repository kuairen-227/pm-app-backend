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
        var result = Role.Create(roleType);

        // Assert
        result.Value.Should().Be(roleType);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = Role.Create(Role.RoleType.Admin);
        var result2 = Role.Create(Role.RoleType.Admin);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var result1 = Role.Create(Role.RoleType.Admin);
        var result2 = Role.Create(Role.RoleType.Manager);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
