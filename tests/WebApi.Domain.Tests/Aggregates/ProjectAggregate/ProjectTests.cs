using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var result = new ProjectBuilder().Build();

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
        Action act = () => new ProjectBuilder().WithName(name!).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PROJECT_NAME_REQUIRED");
    }

    [Fact]
    public void 異常系_インスタンス生成_OwnerIdが空の場合()
    {
        // Arrange & Act
        Action act = () => new ProjectBuilder().WithOwnerId(Guid.Empty).Build();

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("PROJECT_OWNER_ID_REQUIRED");
    }

    [Fact]
    public void 正常系_CreateTicket()
    {
        // Arrange
        var project = new ProjectBuilder().Build();
        var ticket = new TicketBuilder().WithProjectId(project.Id).Build();
        var clock = new FakeDateTimeProvider();

        // Act
        var result = project.CreateTicket(ticket.Title, ticket.Deadline, ticket.CreatedBy, clock);

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
