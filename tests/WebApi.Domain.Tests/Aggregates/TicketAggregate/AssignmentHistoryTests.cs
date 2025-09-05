using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class AssignmentHistoryTests
{
    [Fact]
    public void 正常系_Assigned()
    {
        // Arrange & Act
        var result = AssignmentHistory.Assigned(Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        result.Should().NotBeNull();
        result.ChangeType.Should().Be(AssignmentHistory.AssignmentChangeType.Assigned);
        result.AssigneeId.Should().NotBeNull();
        result.PreviousAssigneeId.Should().BeNull();
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
        var result = AssignmentHistory.Changed(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        result.Should().NotBeNull();
        result.ChangeType.Should().Be(AssignmentHistory.AssignmentChangeType.Changed);
        result.AssigneeId.Should().NotBeNull();
        result.PreviousAssigneeId.Should().NotBeNull();
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
        var result = AssignmentHistory.Unassigned(Guid.NewGuid(), DateTime.UtcNow);

        // Assert
        result.Should().NotBeNull();
        result.ChangeType.Should().Be(AssignmentHistory.AssignmentChangeType.Unassigned);
        result.AssigneeId.Should().BeNull();
        result.PreviousAssigneeId.Should().NotBeNull();
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
        var result1 = AssignmentHistory.Assigned(assigneeId, assignedAt);
        var result2 = AssignmentHistory.Assigned(assigneeId, assignedAt);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var result1 = AssignmentHistory.Assigned(Guid.NewGuid(), DateTime.UtcNow);
        var result2 = AssignmentHistory.Assigned(Guid.NewGuid(), DateTime.UtcNow.AddMinutes(1));

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
