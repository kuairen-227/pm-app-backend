using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.TicketAggregate;

public class TicketTests : BaseDomainTest
{
    private readonly TicketBuilder _ticketBuilder;
    private readonly UserBuilder _userBuilder;
    private readonly TicketCommentBuilder _commentBuilder;

    public TicketTests()
    {
        _ticketBuilder = new TicketBuilder();
        _userBuilder = new UserBuilder();
        _commentBuilder = new TicketCommentBuilder();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _ticketBuilder.Build();

        // Assert
        result.Should().NotBeNull();
        result.Status.Value.Should().Be(TicketStatus.StatusType.Todo);
    }

    [Fact]
    public void 異常系_インスタンス生成_ProjectIdが空の場合()
    {
        // Assert & Build
        var act = () => _ticketBuilder.WithProjectId(Guid.Empty).Build();

        // Then
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.PROJECT_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeTitle()
    {
        // Arrange
        var title = "タイトル - 編集";

        // Act
        var result = _ticketBuilder.Build();
        result.ChangeTitle(title, UserContext.Id);

        // Assert
        result.Title.Value.Should().Be(title);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_ChangeTitle_タイトルが空の場合(string? title)
    {
        // Arrange
        var ticket = _ticketBuilder.Build();

        // Act
        var act = () => ticket.ChangeTitle(title!, UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.TICKET_TITLE_REQUIRED");
    }

    [Fact]
    public void 正常系_Assign()
    {
        // Arrange
        var user = _userBuilder.Build();

        // Act
        var result = _ticketBuilder.Build();
        result.Assign(user.Id, user.Name, [Guid.NewGuid(), Guid.NewGuid()], UserContext.Id);

        // Assert
        result.AssigneeId.Should().Be(user.Id);
        result.AssignmentHistories.Should().HaveCount(1);
    }

    [Fact]
    public void 異常系_Assign_AssigneeIdが空の場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();

        // Act
        var act = () => ticket.Assign(Guid.Empty, "テスト 太郎", [Guid.NewGuid(), Guid.NewGuid()], UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.ASSIGNEE_ID_REQUIRED");
    }

    [Fact]
    public void 異常系_Assign_アサイン済のユーザーの場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var user = _userBuilder.Build();
        ticket.Assign(user.Id, user.Name, [Guid.NewGuid(), Guid.NewGuid()], UserContext.Id);

        // Act
        var act = () => ticket.Assign(user.Id, user.Name, [Guid.NewGuid(), Guid.NewGuid()], UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.ALREADY_ASSIGNED_SAME_USER");
    }

    [Fact]
    public void 正常系_Unassign()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var user = _userBuilder.Build();
        ticket.Assign(user.Id, user.Name, [Guid.NewGuid(), Guid.NewGuid()], UserContext.Id);

        // Act
        ticket.Unassign(UserContext.Id);

        // Assert
        ticket.AssigneeId.Should().BeNull();
    }

    [Fact]
    public void 異常系_Unassign_未アサインの場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();

        // Act
        var act = () => ticket.Unassign(UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NOT_ASSIGNED");
    }

    [Fact]
    public void 正常系_ChangeSchedule()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var startDate = DateOnly.Parse("2025-01-01");
        var endDate = DateOnly.Parse("2025-12-31");

        // Act
        ticket.ChangeSchedule(startDate, endDate, UserContext.Id);

        // Assert
        ticket.Schedule.StartDate.Should().Be(startDate);
        ticket.Schedule.EndDate.Should().Be(endDate);
    }

    [Fact]
    public void 正常系_ChangeStatus()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();

        // Act
        ticket.ChangeStatus(TicketStatus.StatusType.InProgress, UserContext.Id);

        // Assert
        ticket.Status.Value.Should().Be(TicketStatus.StatusType.InProgress);
    }

    [Fact]
    public void 正常系_SetCompletionCriteria()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var criteria = "完了条件";

        // Act
        ticket.SetCompletionCriteria(criteria, UserContext.Id);

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
        var ticket = _ticketBuilder.Build();

        // Act
        var act = () => ticket.SetCompletionCriteria(criteria!, UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.COMPLETION_CRITERIA_REQUIRED");
    }

    [Fact]
    public void 正常系_AddComment_1件()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var author = _userBuilder.Build();
        var content = "コメント";
        var createdBy = author.Id;

        // Act
        ticket.AddComment(author.Id, content, createdBy);

        // Assert
        ticket.Comments.Should().ContainSingle();
        ticket.Comments.First().AuthorId.Should().Be(author.Id);
        ticket.Comments.First().Content.Should().Be(content);
        ticket.Comments.First().AuditInfo.CreatedBy.Should().Be(createdBy);
        ticket.Comments.First().AuditInfo.CreatedAt.Should().Be(Clock.Now);
    }

    [Fact]
    public void 正常系_AddComment_2件()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var authors = new[] { _userBuilder.Build(), _userBuilder.Build() };
        var contents = new[] { "コメント1", "コメント2" };
        var createdBys = new[] { authors[0].Id, authors[1].Id };

        // Act
        ticket.AddComment(authors[0].Id, contents[0], createdBys[0]);
        ticket.AddComment(authors[1].Id, contents[1], createdBys[1]);

        // Assert
        ticket.Comments.Should().HaveCount(2);
        ticket.Comments.ElementAt(0).AuthorId.Should().Be(authors[0].Id);
        ticket.Comments.ElementAt(1).AuthorId.Should().Be(authors[1].Id);
        ticket.Comments.ElementAt(0).Content.Should().Be(contents[0]);
        ticket.Comments.ElementAt(1).Content.Should().Be(contents[1]);
        ticket.Comments.ElementAt(0).AuditInfo.CreatedBy.Should().Be(createdBys[0]);
        ticket.Comments.ElementAt(1).AuditInfo.CreatedBy.Should().Be(createdBys[1]);
        ticket.Comments.ElementAt(0).AuditInfo.CreatedAt.Should().Be(Clock.Now);
        ticket.Comments.ElementAt(1).AuditInfo.CreatedAt.Should().Be(Clock.Now);
    }

    [Fact]
    public void 正常系_EditComment()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var comment = ticket.AddComment(Guid.NewGuid(), "コメント1", Guid.NewGuid());

        // Act
        ticket.EditComment(comment.Id, comment.AuthorId, "コメント1-編集", UserContext.Id);

        // Assert
        ticket.Comments.Should().ContainSingle();
        ticket.Comments.First().Content.Should().Be("コメント1-編集");
    }

    [Fact]
    public void 異常系_EditComment_存在しないコメントの場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var comment = _commentBuilder.Build();

        // Act
        var act = () => ticket.EditComment(
            comment.Id, comment.AuthorId, "コメント1-編集", UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.TICKET_COMMENT_NOT_FOUND");
    }

    [Fact]
    public void 異常系_EditComment_作成者以外が編集しようとした場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var comment = ticket.AddComment(Guid.NewGuid(), "コメント", Guid.NewGuid());

        // Act
        var act = () => ticket.EditComment(
            comment.Id, Guid.NewGuid(), "コメント-編集", UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NOT_TICKET_COMMENT_AUTHOR");
    }

    [Fact]
    public void 正常系_DeleteComment()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var comment1 = ticket.AddComment(Guid.NewGuid(), "コメント1", Guid.NewGuid());
        var comment2 = ticket.AddComment(Guid.NewGuid(), "コメント2", Guid.NewGuid());

        // Act
        ticket.DeleteComment(comment1.Id, comment1.AuthorId);

        // Assert
        ticket.Comments.Should().ContainSingle();
        ticket.Comments.First().Content.Should().Be("コメント2");
    }

    [Fact]
    public void 異常系_DeleteComment_存在しないコメントの場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var comment = _commentBuilder.Build();

        // Act
        var act = () => ticket.DeleteComment(comment.Id, comment.AuthorId);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.TICKET_COMMENT_NOT_FOUND");
    }

    [Fact]
    public void 異常系_DeleteComment_作成者以外が削除しようとした場合()
    {
        // Arrange
        var ticket = _ticketBuilder.Build();
        var comment = ticket.AddComment(Guid.NewGuid(), "コメント", Guid.NewGuid());

        // Act
        var act = () => ticket.DeleteComment(comment.Id, Guid.NewGuid());

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.NOT_TICKET_COMMENT_AUTHOR");
    }
}
