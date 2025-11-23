using FluentAssertions;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common;
using WebApi.Domain.Tests.Helpers;
using WebApi.Tests.Helpers.Builders;

namespace WebApi.Domain.Tests.Aggregates.ProjectAggregate;

public class ProjectTests : BaseDomainTest
{
    private readonly ProjectBuilder _projectBuilder;
    private readonly ProjectMemberBuilder _projectMemberBuilder;
    private readonly UserBuilder _userBuilder;

    public ProjectTests()
    {
        _projectBuilder = new ProjectBuilder();
        _projectMemberBuilder = new ProjectMemberBuilder();
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
        ex.Which.ErrorCode.Should().Be("DOMAIN.PROJECT_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_Rename()
    {
        // Arrange
        var newName = "プロジェクト名 - 更新後";

        // Act
        var result = _projectBuilder.Build();
        result.Rename(newName, Guid.NewGuid());

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
        var act = () => result.Rename(newName!, Guid.NewGuid());

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.PROJECT_NAME_REQUIRED");
    }

    [Fact]
    public void 正常系_ChangeDescription()
    {
        // Arrange
        var newDescription = "プロジェクトの説明 - 更新後";

        // Act
        var result = _projectBuilder.Build();
        result.ChangeDescription(newDescription, Guid.NewGuid());

        // Assert
        result.Description.Should().Be(newDescription);
    }

    [Fact]
    public void 正常系_InviteMember()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.RoleType.Member;

        // Act
        var result = _projectBuilder.Build();
        result.InviteMember(user.Id, role, UserContext.Id);

        // Assert
        result.Members.Count.Should().Be(1);
        result.Members[0].UserId.Should().Be(user.Id);
        result.Members[0].Role.Value.Should().Be(role);
    }

    [Fact]
    public void 異常系_InviteMember_すでに参画済のユーザーの場合()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var member = _projectMemberBuilder
            .WithUserId(user.Id)
            .WithRole(role.Value)
            .Build();
        var project = _projectBuilder.WithMembers(member).Build();

        // Act
        var act = () => project.InviteMember(user.Id, role.Value, UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_ALREADY_JOINED");
    }

    [Fact]
    public void 正常系_RemoveMember()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var member = _projectMemberBuilder
            .WithUserId(user.Id)
            .WithRole(role.Value)
            .Build();
        var result = _projectBuilder.WithMembers(member).Build();

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
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_NOT_PROJECT_MEMBER");
    }

    [Fact]
    public void 正常系_ChangeMemberRole()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var member = _projectMemberBuilder
            .WithUserId(user.Id)
            .WithRole(role.Value)
            .Build();
        var result = _projectBuilder.WithMembers(member).Build();

        // Act
        var newRole = ProjectRole.RoleType.ProjectManager;
        result.ChangeMemberRole(user.Id, newRole, UserContext.Id);

        // Assert
        result.Members.Count.Should().Be(1);
        result.Members[0].Role.Value.Should().Be(newRole);
    }

    [Fact]
    public void 異常系_ChangeMemberRole()
    {
        // Arrange
        var project = _projectBuilder.Build();
        var role = ProjectRole.RoleType.Member;

        // Act
        var act = () => project.ChangeMemberRole(Guid.NewGuid(), role, UserContext.Id);

        // Assert
        var ex = act.Should().Throw<DomainException>();
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_NOT_PROJECT_MEMBER");
    }

    [Fact]
    public void 正常系_EnsureMember()
    {
        // Arrange
        var user = _userBuilder.Build();
        var role = ProjectRole.Create(ProjectRole.RoleType.Member);
        var member = _projectMemberBuilder
            .WithUserId(user.Id)
            .WithRole(role.Value)
            .Build();
        var project = _projectBuilder.WithMembers(member).Build();

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
        ex.Which.ErrorCode.Should().Be("DOMAIN.USER_NOT_PROJECT_MEMBER");
    }
}
