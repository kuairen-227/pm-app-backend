using FluentAssertions;
using WebApi.Domain.Aggregates.TicketAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectTests
{
    [Fact]
    public void 正常系_インスタンス生成()
    {
        // Arrange & Act
        var project = new ProjectBuilder().Build();

        // Assert
        project.Should().NotBeNull();
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
        var ticketTitle = TicketTitle.Create("チケットタイトル");
        var ticketDeadline = Deadline.Create(DateTime.UtcNow.AddDays(7));

        // Act
        var ticket = project.CreateTicket(ticketTitle, ticketDeadline);

        // Assert
        ticket.Should().NotBeNull();
        ticket.Title.Should().Be(ticketTitle);
        ticket.Deadline.Should().Be(ticketDeadline);
    }
}
