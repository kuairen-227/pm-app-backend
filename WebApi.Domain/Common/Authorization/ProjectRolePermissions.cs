using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Common.Authorization.Permissions;

namespace WebApi.Domain.Common.Authorization;

public static class ProjectRolePermissions
{
    public static readonly IReadOnlyDictionary<ProjectRole.RoleType, IReadOnlyCollection<string>> Map =
        new Dictionary<ProjectRole.RoleType, IReadOnlyCollection<string>>
        {
            {
                ProjectRole.RoleType.ProjectManager, new []
                {
                    ProjectPermissions.View, ProjectPermissions.Update,
                    ProjectPermissions.InviteMember, ProjectPermissions.RemoveMember, ProjectPermissions.ChangeMemberRole,
                    TicketPermissions.View, TicketPermissions.Create, TicketPermissions.Update, TicketPermissions.Delete,
                    TicketPermissions.Assign, TicketPermissions.Unassign,
                }
            },
            {
                ProjectRole.RoleType.Member, new[]
                {
                    TicketPermissions.View, TicketPermissions.Update
                }
            }
        };
}
