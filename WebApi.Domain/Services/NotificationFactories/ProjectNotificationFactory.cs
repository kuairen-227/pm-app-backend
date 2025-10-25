using WebApi.Domain.Abstractions;
using WebApi.Domain.Aggregates.NotificationAggregate;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Domain.Services.NotificationFactories;

public sealed class ProjectNotificationFactory
{
    public readonly IDateTimeProvider _clock;

    public ProjectNotificationFactory(IDateTimeProvider clock)
    {
        _clock = clock;
    }

    public Notification CreateMemberInvitedNotification(
        Guid recipientId, Guid projectId, string projectName, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.ProjectMemberInvited,
            projectId,
            $"{projectName} に招待されました。",
            createdBy,
            _clock
        );
    }

    public Notification CreateProjectRoleChangedNotification(
        Guid recipientId, Guid memberId, ProjectRole.RoleType newRoleType, Guid createdBy)
    {
        return new Notification(
            recipientId,
            NotificationCategory.Category.ProjectMemberRoleChanged,
            memberId,
            $"メンバー権限が {newRoleType} に変更されました。",
            createdBy,
            _clock
        );
    }
}
