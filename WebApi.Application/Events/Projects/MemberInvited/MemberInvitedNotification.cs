using MediatR;

namespace WebApi.Application.Events.Projects.MemberInvited;

public sealed class MemberInvitedNotification : INotification
{
    public Guid ProjectId { get; }
    public string ProjectName { get; }
    public Guid UserId { get; }

    public MemberInvitedNotification(Guid projectId, string projectName, Guid userId)
    {
        ProjectId = projectId;
        ProjectName = projectName;
        UserId = userId;
    }
}
