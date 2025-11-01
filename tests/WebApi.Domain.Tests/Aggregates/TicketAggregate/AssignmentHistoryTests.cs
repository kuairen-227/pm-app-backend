using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class AssignmentHistoryTests : BaseDomainTest
{
    [Fact]
    public void 正常系_Assigned()
    {
        // Arrange & Act
        var result = AssignmentHistory.Assigned(Guid.NewGuid(), Guid.NewGuid(), Clock);

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
        var act = () => AssignmentHistory.Assigned(Guid.Empty, Guid.NewGuid(), Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_Changed()
    {
        // Arrange & Act
        var result = AssignmentHistory.Changed(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Clock);

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
        var act = () => AssignmentHistory.Changed(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 異常系_Changed_PreviousAssigneeIdが空の場合()
    {
        // Arrange & Act
        var act = () => AssignmentHistory.Changed(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("PREVIOUS_ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_Unassigned()
    {
        // Arrange & Act
        var result = AssignmentHistory.Unassigned(Guid.NewGuid(), Guid.NewGuid(), Clock);

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
        var act = () => AssignmentHistory.Unassigned(Guid.Empty, Guid.NewGuid(), Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("PREVIOUS_ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_値が同じ場合()
    {
        // Arrange
        var assigneeId = Guid.NewGuid();
        var assignedAt = DateTime.UtcNow;

        // Act
        var result1 = AssignmentHistory.Assigned(assigneeId, Guid.NewGuid(), Clock);
        var result2 = AssignmentHistory.Assigned(assigneeId, Guid.NewGuid(), Clock);

        // Assert
        result1.Should().Be(result2);
        result1.GetHashCode().Should().Be(result2.GetHashCode());
        result1.Equals(result2).Should().BeTrue();
    }

    [Fact]
    public void 正常系_値が異なる場合()
    {
        // Arrange & Act
        var result1 = AssignmentHistory.Assigned(Guid.NewGuid(), Guid.NewGuid(), Clock);
        var result2 = AssignmentHistory.Assigned(Guid.NewGuid(), Guid.NewGuid(), Clock);

        // Assert
        result1.Should().NotBe(result2);
        result1.GetHashCode().Should().NotBe(result2.GetHashCode());
        result1.Equals(result2).Should().BeFalse();
    }
}
