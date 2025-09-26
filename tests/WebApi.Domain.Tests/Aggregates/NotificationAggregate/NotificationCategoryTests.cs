using FluentAssertions;
using WebApi.Domain.Aggregates.NotificationAggregate;

namespace WebApi.Domain.Tests.Aggregates.NotificationAggregate;

public class NotificationCategoryTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange
        var category = NotificationCategory.Category.ProjectInvitation;

        // Act
        var result = NotificationCategory.Create(category);

        // Assert
        result.Value.Should().Be(category);
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange
        var category1 = NotificationCategory.Category.ProjectInvitation;
        var category2 = NotificationCategory.Category.ProjectInvitation;

        // Act
        var result1 = NotificationCategory.Create(category1);
        var result2 = NotificationCategory.Create(category2);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange
        var category1 = NotificationCategory.Category.ProjectInvitation;
        var category2 = NotificationCategory.Category.ProjectChangeMemberRole;

        // Act
        var result1 = NotificationCategory.Create(category1);
        var result2 = NotificationCategory.Create(category2);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
