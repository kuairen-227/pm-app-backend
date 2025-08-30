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
        ticket.AssignmentHistories.Should().HaveCount(1);
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

    [Fact]
    public void 正常系_AddComment_1件()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();

        // Act
        ticket.AddComment(Guid.NewGuid(), "コメント");

        // Assert
        ticket.Comments.Should().ContainSingle();
        ticket.Comments.First().Content.Should().Be("コメント");
    }

    [Fact]
    public void 正常系_AddComment_2件()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();

        // Act
        ticket.AddComment(Guid.NewGuid(), "コメント1");
        ticket.AddComment(Guid.NewGuid(), "コメント2");

        // Assert
        ticket.Comments.Should().HaveCount(2);
        ticket.Comments.ElementAt(0).Content.Should().Be("コメント1");
        ticket.Comments.ElementAt(1).Content.Should().Be("コメント2");
    }

    [Fact]
    public void 正常系_RemoveComment()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();
        var comment1 = ticket.AddComment(Guid.NewGuid(), "コメント1");
        var comment2 = ticket.AddComment(Guid.NewGuid(), "コメント2");

        // Act
        ticket.RemoveComment(comment1);

        // Assert
        ticket.Comments.Should().ContainSingle();
        ticket.Comments.First().Content.Should().Be("コメント2");
    }

    [Fact]
    public void 異常系_RemoveComment_存在しないコメントの場合()
    {
        // Arrange
        var ticket = new TicketBuilder().Build();
        var comment = new TicketCommentBuilder().Build();

        // Act
        Action act = () => ticket.RemoveComment(comment);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("COMMENT_NOT_FOUND");
    }
}
