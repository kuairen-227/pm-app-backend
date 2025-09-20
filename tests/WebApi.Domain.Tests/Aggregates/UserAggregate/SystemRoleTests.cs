using FluentAssertions;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Domain.Tests.Aggregates.UserAggregate;

public class RoleTests
{
    [Theory]
    [InlineData(SystemRole.RoleType.Admin)]
    [InlineData(SystemRole.RoleType.User)]
    public void 正常系_インスタンス生成(SystemRole.RoleType roleType)
    {
        // Act
        var result = SystemRole.Create(roleType);

        // Assert
        result.Value.Should().Be(roleType);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange & Act
        var result1 = SystemRole.Create(SystemRole.RoleType.Admin);
        var result2 = SystemRole.Create(SystemRole.RoleType.Admin);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var result1 = SystemRole.Create(SystemRole.RoleType.Admin);
        var result2 = SystemRole.Create(SystemRole.RoleType.User);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
