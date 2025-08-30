using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class TicketTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var ticket = new TicketBuilder().Build();

        // Assert
        ticket.Should().NotBeNull();
        ticket.Status.Value.Should().Be(TicketStatus.StatusType.Todo);
    }

    [Fact]
    public void 異常系_インスタンス生成_ProjectIdが空の場合()
    {
        // Assert & Build
        Action act = () => new TicketBuilder().WithProjectId(Guid.Empty).Build();

        // Then
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PROJECT_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_Assign()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();
        var user = new UserBuilder().Build();

        // Act
        ticket.Assign(user.Id);

        // Assert
        ticket.AssigneeId.Should().Be(user.Id);
    }

    [Fact]
    public void 異常系_Assign_AssigneeIdが空の場合()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();

        // Act
        Action act = () => ticket.Assign(Guid.Empty);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 異常系_Assign_アサイン済のユーザーの場合()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();
        var user = new UserBuilder().Build();
        ticket.Assign(user.Id);

        // Act
        Action act = () => ticket.Assign(user.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("ALREADY_ASSIGNED_SAME_USER");
    }

    [Fact]
    public void 正常系_UnAssign()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();
        var user = new UserBuilder().Build();
        ticket.Assign(user.Id);

        // Act
        ticket.UnAssign();

        // Assert
        ticket.AssigneeId.Should().BeNull();
    }

    [Fact]
    public void 異常系_UnAssign_未アサインの場合()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();

        // Act
        Action act = () => ticket.UnAssign();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("NOT_ASSIGNED");
    }

    [Fact]
    public void 正常系_ChangeStatus()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();

        // Act
        ticket.ChangeStatus(TicketStatus.StatusType.InProgress);

        // Assert
        ticket.Status.Value.Should().Be(TicketStatus.StatusType.InProgress);
    }

    [Fact]
    public void 正常系_SetCompletionCriteria()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();
        var criteria = "完了条件";

        // Act
        ticket.SetCompletionCriteria(criteria);

        // Assert
        ticket.CompletionCriteria.Should().Be(criteria);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_SetCompletionCriteria_空の場合(string? criteria)
    {
        // Arrange
        var ticket = new TicketBuilder().Build();

        // Act
        Action act = () => ticket.SetCompletionCriteria(criteria!);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("COMPLETION_CRITERIA_REQUIRED");
    }
}
