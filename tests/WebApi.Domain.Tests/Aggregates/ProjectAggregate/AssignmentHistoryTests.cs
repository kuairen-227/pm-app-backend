using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class AssignmentHistoryTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var assignmentHistory = AssignmentHistory.Create(Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        assignmentHistory.Should().NotBeNull();
    }

    [Fact]
    public void 異常系_インスタンス生成_AssigneeIdが空の場合()
    {
        // Arrange & Act
        Action act = () => AssignmentHistory.Create(Guid.Empty, DateTime.UtcNow);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange
        var assigneeId = Guid.NewGuid();
        var assignedAt = DateTime.UtcNow;

        // Act
        var assignmentHistory1 = AssignmentHistory.Create(assigneeId, assignedAt);
        var assignmentHistory2 = AssignmentHistory.Create(assigneeId, assignedAt);

        // Assert
        assignmentHistory1.Should().Be(assignmentHistory2);
        assignmentHistory1.GetHashCode().Should().Be(assignmentHistory2.GetHashCode());
        assignmentHistory1.Equals(assignmentHistory2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var assignmentHistory1 = AssignmentHistory.Create(Guid.NewGuid(), DateTime.UtcNow);
        var assignmentHistory2 = AssignmentHistory.Create(Guid.NewGuid(), DateTime.UtcNow.AddMinutes(1));

        // Assert
        assignmentHistory1.Should().NotBe(assignmentHistory2);
        assignmentHistory1.GetHashCode().Should().NotBe(assignmentHistory2.GetHashCode());
        assignmentHistory1.Equals(assignmentHistory2).Should().BeFalse();
    }
}
