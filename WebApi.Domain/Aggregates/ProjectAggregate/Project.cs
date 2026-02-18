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

    public void InviteMember(
        Guid userId, ProjectRole.RoleType roleType, Guid invitedBy, IDateTimeProvider clock)
    {
        if (_members.Any(m => m.UserId == userId))
            throw new DomainException("USER_ALREADY_JOINED", "User はすでにプロジェクトメンバーです");

        var role = ProjectRole.Create(roleType);
        var member = new ProjectMember(userId, role, invitedBy, clock);
        _members.Add(member);

        AddDomainEvent(new ProjectMemberInvitedEvent(Id, Name, userId, clock));
    }

    public void RemoveMember(Guid userId)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId)
            ?? throw new DomainException("USER_NOT_PROJECT_MEMBER", "User はプロジェクトメンバーではありません");

        _members.Remove(member);
    }

    public void ChangeMemberRole(
        Guid userId, ProjectRole.RoleType newRoleType, Guid changedBy, IDateTimeProvider clock)
    {
        var member = _members.FirstOrDefault(m => m.UserId == userId)
            ?? throw new DomainException("USER_NOT_PROJECT_MEMBER", "User はプロジェクトメンバーではありません");

        member.ChangeRole(ProjectRole.Create(newRoleType), changedBy, clock);
        AddDomainEvent(new ProjectRoleChangedEvent(Id, userId, newRoleType, clock));
    }

    public void EnsureMember(Guid userId)
    {
        if (!_members.Any(m => m.UserId == userId))
            throw new DomainException("USER_NOT_PROJECT_MEMBER", "User はプロジェクトメンバーではありません");
    }
}
