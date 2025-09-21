using WebApi.Domain.Abstractions;
using WebApi.Domain.Common;

namespace WebApi.Domain.Aggregates.ProjectAggregate;

public sealed class Project : Entity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private readonly List<ProjectMember> _members = new();
    public IReadOnlyList<ProjectMember> Members => _members.AsReadOnly();

    public Project(
        string name, string? description, Guid createdBy, IDateTimeProvider clock)
        : base(createdBy, clock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Name は必須です");

        Name = name;
        Description = description;
    }

    public void Rename(string newName, Guid updatedBy, IDateTimeProvider clock)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("PROJECT_NAME_REQUIRED", "Project Name は必須です");

        Name = newName;
        UpdateAuditInfo(updatedBy, clock);
    }

    public void ChangeDescription(string? newDescription, Guid updatedBy, IDateTimeProvider clock)
    {
        Description = newDescription;
        UpdateAuditInfo(updatedBy, clock);
    }

    public void InviteMember(Guid userId, ProjectRole.RoleType roleType)
    {
        if (_members.Any(m => m.UserId == userId))
            throw new DomainException("USER_ALREADY_JOINED", "User はすでにプロジェクトメンバーです");

        var role = ProjectRole.Create(roleType);
        var member = ProjectMember.Create(userId, role);
        _members.Add(member);
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
    }

    public void EnsureMember(Guid userId)
    {
        if (!_members.Any(m => m.UserId == userId))
            throw new DomainException("USER_NOT_PROJECT_MEMBER", "User はプロジェクトメンバーではありません");
    }
}
