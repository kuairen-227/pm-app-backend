using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class AssignmentHistoryTests
{
    [Fact]
    public void 正常系_Assigned()
    {
        // Arrange & Act
        var assignmentHistory = AssignmentHistory.Assigned(Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        assignmentHistory.Should().NotBeNull();
        assignmentHistory.ChangeType.Should().Be(AssignmentHistory.AssignmentChangeType.Assigned);
        assignmentHistory.AssigneeId.Should().NotBeNull();
        assignmentHistory.PreviousAssigneeId.Should().BeNull();
    }

    [Fact]
    public void 異常系_Assigned_AssigneeIdが空の場合()
    {
        // Arrange & Act
        Action act = () => AssignmentHistory.Assigned(Guid.Empty, DateTime.UtcNow);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_Changed()
    {
        // Arrange & Act
        var assignmentHistory = AssignmentHistory.Changed(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        assignmentHistory.Should().NotBeNull();
        assignmentHistory.ChangeType.Should().Be(AssignmentHistory.AssignmentChangeType.Changed);
        assignmentHistory.AssigneeId.Should().NotBeNull();
        assignmentHistory.PreviousAssigneeId.Should().NotBeNull();
    }

    [Fact]
    public void 異常系_Changed_AssigneeIdが空の場合()
    {
        // Arrange & Act
        Action act = () => AssignmentHistory.Changed(Guid.Empty, Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 異常系_Changed_PreviousAssigneeIdが空の場合()
    {
        // Arrange & Act
        Action act = () => AssignmentHistory.Changed(Guid.NewGuid(), Guid.Empty, DateTime.UtcNow);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PREVIOUS_ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_Unassigned()
    {
        // Arrange & Act
        var assignmentHistory = AssignmentHistory.Unassigned(Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        assignmentHistory.Should().NotBeNull();
        assignmentHistory.ChangeType.Should().Be(AssignmentHistory.AssignmentChangeType.Unassigned);
        assignmentHistory.AssigneeId.Should().BeNull();
        assignmentHistory.PreviousAssigneeId.Should().NotBeNull();
    }

    [Fact]
    public void 異常系_Unassigned_PreviousAssigneeIdが空の場合()
    {
        // Arrange & Act
        Action act = () => AssignmentHistory.Unassigned(Guid.Empty, DateTime.UtcNow);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PREVIOUS_ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange
        var assigneeId = Guid.NewGuid();
        var assignedAt = DateTime.UtcNow;

        // Act
        var assignmentHistory1 = AssignmentHistory.Assigned(assigneeId, assignedAt);
        var assignmentHistory2 = AssignmentHistory.Assigned(assigneeId, assignedAt);

        // Assert
        assignmentHistory1.Should().Be(assignmentHistory2);
        assignmentHistory1.GetHashCode().Should().Be(assignmentHistory2.GetHashCode());
        assignmentHistory1.Equals(assignmentHistory2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var assignmentHistory1 = AssignmentHistory.Assigned(Guid.NewGuid(), DateTime.UtcNow);
        var assignmentHistory2 = AssignmentHistory.Assigned(Guid.NewGuid(), DateTime.UtcNow.AddMinutes(1));

        // Assert
        assignmentHistory1.Should().NotBe(assignmentHistory2);
        assignmentHistory1.GetHashCode().Should().NotBe(assignmentHistory2.GetHashCode());
        assignmentHistory1.Equals(assignmentHistory2).Should().BeFalse();
    }
}
