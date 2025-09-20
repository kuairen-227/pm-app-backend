using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectTests : BaseDomainTest
{
    private readonly ProjectBuilder _projectBuilder;
    private readonly UserBuilder _userBuilder;

    public ProjectTests()
    {
        _projectBuilder = new ProjectBuilder();
        _userBuilder = new UserBuilder();
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
        var act = () => _projectBuilder.WithName(name!).Build();

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
        var result = _projectBuilder.Build();

        // Act
        var act = () => result.Rename(newName!, Guid.NewGuid(), Clock);

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

    [Fact]
    public void 正常系_InviteMember()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Act
        var result = _projectBuilder.Build();
        result.InviteMember(user.Id, role);

        // Assert
        var member = ProjectMember.Create(user.Id, role);
        result.Members.Count.Should().Be(1);
        result.Members[0].Should().Be(member);
    }

    [Fact]
    public void 異常系_InviteMember_すでに参画済のユーザーの場合()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var project = _projectBuilder
            .WithMembers(ProjectMember.Create(user.Id, role))
            .Build();

        // Act
        var act = () => project.InviteMember(user.Id, role);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("USER_ALREADY_JOINED");
    }

    [Fact]
    public void 正常系_RemoveMember()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var result = _projectBuilder
            .WithMembers(ProjectMember.Create(user.Id, role))
            .Build();

        // Act
        result.RemoveMember(user.Id);

        // Assert
        result.Members.Count.Should().Be(0);
    }

    [Fact]
    public void 異常系_RemoveMember()
    {
        // Arrange
        var project = _projectBuilder.Build();

        // Act
        var act = () => project.RemoveMember(Guid.NewGuid());

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("USER_NOT_PROJECT_MEMBER");
    }

    [Fact]
    public void 正常系_ChangeMemberRole()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var result = _projectBuilder
            .WithMembers(ProjectMember.Create(user.Id, role))
            .Build();

        // Act
        var newRole = ProjectRole.Create(ProjectRole.RoleType.ProjectManager);
        result.ChangeMemberRole(user.Id, newRole);

        // Assert
        var member = ProjectMember.Create(user.Id, newRole);
        result.Members.Count.Should().Be(1);
        result.Members[0].Should().Be(member);
    }

    [Fact]
    public void 異常系_ChangeMemberRole()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);

        // Act
        var act = () => project.ChangeMemberRole(Guid.NewGuid(), role);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("USER_NOT_PROJECT_MEMBER");
    }

    [Fact]
    public void 正常系_EnsureMember()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var project = _projectBuilder
            .WithMembers(ProjectMember.Create(user.Id, role))
            .Build();

        // Act
        var act = () => project.EnsureMember(user.Id);

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void 異常系_EnsureMember()
    {
        // Arrange
        var project = _projectBuilder.Build();

        // Act
        var act = () => project.EnsureMember(Guid.NewGuid());

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("USER_NOT_PROJECT_MEMBER");
    }
}
