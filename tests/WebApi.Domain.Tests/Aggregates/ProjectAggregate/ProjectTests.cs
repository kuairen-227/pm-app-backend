using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectTests
{
    private readonly ProjectBuilder _projectBuilder;
    private readonly TicketBuilder _ticketBuilder;
    private readonly FakeDateTimeProvider _clock;

    public ProjectTests()
    {
        _projectBuilder = new ProjectBuilder();
        _ticketBuilder = new TicketBuilder();
        _clock = new FakeDateTimeProvider();
    }

    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = _projectBuilder.Build();

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_インスタンス生成_ProjectNameが空の場合(string? name)
    {
        // Arrange & Act
        Action act = () => _projectBuilder.WithName(name!).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PROJECT_NAME_REQUIRED");
    }

    [Fact]
    public void 異常系_インスタンス生成_OwnerIdが空の場合()
    {
        // Arrange & Act
        Action act = () => _projectBuilder.WithOwnerId(Guid.Empty).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PROJECT_OWNER_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_Rename()
    {
        // Arrange
        var newName = "プロジェクト名 - 更新後";

        // Act
        var result = _projectBuilder.Build();
        result.Rename(newName, Guid.NewGuid(), _clock);

        // Assert
        result.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void 異常系_Rename_ProjectNameが空の場合(string? newName)
    {
        // Arrange
        var project = _projectBuilder.Build();

        // Act
        Action act = () => project.Rename(newName!, Guid.NewGuid(), _clock);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PROJECT_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeDescription()
    {
        // Arrange
        var newDescription = "プロジェクトの説明 - 更新後";

        // Act
        var result = _projectBuilder.Build();
        result.ChangeDescription(newDescription, Guid.NewGuid(), _clock);

        // Assert
        result.Description.Should().Be(newDescription);
    }

    [Fact]
    public void 正常系_ChangeOwner()
    {
        // Arrange
        var newOwnerId = Guid.NewGuid();

        // Act
        var result = _projectBuilder.Build();
        result.ChangeOwner(newOwnerId, Guid.NewGuid(), _clock);

        // Assert
        result.OwnerId.Should().Be(newOwnerId);
    }

    [Fact]
    public void 正常系_CreateTicket()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var ticket = _ticketBuilder.WithProjectId(project.Id).Build();

        // Act
        var result = project.CreateTicket(ticket.Title, ticket.Deadline, ticket.CreatedBy, _clock);

        // Assert
        result.Should().NotBeNull();
        result.ProjectId.Should().Be(project.Id);
        result.Title.Should().Be(ticket.Title);
        result.AssigneeId.Should().Be(ticket.AssigneeId);
        result.Deadline.Should().Be(ticket.Deadline);
        result.Status.Should().Be(ticket.Status);
        result.CompletionCriteria.Should().Be(ticket.CompletionCriteria);
        result.CreatedBy.Should().Be(ticket.CreatedBy);
        result.CreatedAt.Should().Be(ticket.CreatedAt);
    }
}
