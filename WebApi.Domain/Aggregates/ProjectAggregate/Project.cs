using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Project : Entity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    private readonly List<ProjectMember> _members = new();
    public IReadOnlyList<ProjectMember> Members => _members.AsReadOnly();

    private Project() { } // EF Core 用

    public Project(
        string name, string? description, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Name は必須です");

        Name = name;
        Description = description;
    }

    public void Rename(string newName, Guid updatedBy)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Project Name は必須です");

        Name = newName;
        UpdateAuditInfo(updatedBy);
    }

    public void ChangeDescription(string? newDescription, Guid updatedBy)
    {
        Description = newDescription;
        UpdateAuditInfo(updatedBy);
    }

    public void InviteMember(Guid userId, ProjectRole.RoleType roleType)
    {
        if (_members.Any(m => m.UserId == userId))
            throw new DomainException("USER_ALREADY_JOINED", "User はすでにプロジェクトメンバーです");

        var role = ProjectRole.Create(roleType);
        var member = ProjectMember.Create(userId, role);
        _members.Add(member);

        AddDomainEvent(new ProjectMemberInvitedEvent(Id, Name, userId, _clock));
    }

    public void RemoveMember(Guid userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId)
            ?? throw new DomainException("USER_NOT_PROJECT_MEMBER", "User はプロジェクトメンバーではありません");

        _members.Remove(member);
    }

    public void ChangeMemberRole(Guid userId, ProjectRole.RoleType newRoleType)
    {
        var index = _members.FindIndex(m => m.UserId == userId);
        if (index < 0)
            throw new DomainException("USER_NOT_PROJECT_MEMBER", "User はプロジェクトメンバーではありません");

        var role = ProjectRole.Create(newRoleType);
        _members[index] = ProjectMember.Create(userId, role);

        AddDomainEvent(new ProjectRoleChangedEvent(Id, userId, newRoleType, _clock));
    }

    public void EnsureMember(Guid userId)
    {
        if (!_members.Any(m => m.UserId == userId))
            throw new DomainException("USER_NOT_PROJECT_MEMBER", "User はプロジェクトメンバーではありません");
    }
}
