using FluentAssertions;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Domain.Tests.Helpers.Common;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectTests : BaseDomainTest
{
    private readonly ProjectBuilder _projectBuilder;
    private readonly TicketBuilder _ticketBuilder;

    public ProjectTests()
    {
        _projectBuilder = new ProjectBuilder();
        _ticketBuilder = new TicketBuilder();
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
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("PROJECT_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_Rename()
    {
        // Arrange
        var newName = "プロジェクト名 - 更新後";

        // Act
        var result = _projectBuilder.Build();
        result.Rename(newName, Guid.NewGuid(), Clock);

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
        Action act = () => project.Rename(newName!, Guid.NewGuid(), Clock);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("PROJECT_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeDescription()
    {
        // Arrange
        var newDescription = "プロジェクトの説明 - 更新後";

        // Act
        var result = _projectBuilder.Build();
        result.ChangeDescription(newDescription, Guid.NewGuid(), Clock);

        // Assert
        result.Description.Should().Be(newDescription);
    }
}
