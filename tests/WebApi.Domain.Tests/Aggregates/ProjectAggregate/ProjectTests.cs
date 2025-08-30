using FluentAssertions;
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
    public void 正常系_AddTicket_1件()
    {
        // Arrange
        var project = new ProjectBuilder().Build();
        var ticket = new TicketBuilder().Build();

        // Act
        project.AddTicket(ticket);

        // Assert
        project.Tickets.Should().Contain(ticket);
    }

    [Fact]
    public void 正常系_AddTicket_2件()
    {
        // Arrange
        var project = new ProjectBuilder().Build();
        var ticket1 = new TicketBuilder().Build();
        var ticket2 = new TicketBuilder().Build();

        // Act
        project.AddTicket(ticket1);
        project.AddTicket(ticket2);

        // Assert
        project.Tickets.Should().Contain(new[] { ticket1, ticket2 });
    }

    [Fact]
    public void 異常系_AddTicket_チケットがnullの場合()
    {
        // Arrange
        var project = new ProjectBuilder().Build();

        // Act
        Action act = () => project.AddTicket(null!);

        // Assert
        var ex = act.Should().Throw<DomainException>().Which;
        ex.ErrorCode.Should().Be("TICKET_REQUIRED");
    }
}
