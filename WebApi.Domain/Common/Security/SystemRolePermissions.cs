using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Domain.Common.Security;

public static class SystemRolePermissions
{
    public static readonly IReadOnlyDictionary<SystemRole.RoleType, IReadOnlyCollection<string>> Map =
        new Dictionary<SystemRole.RoleType, IReadOnlyCollection<string>>
        {
            {
                SystemRole.RoleType.Admin, new []
                {
                    UserPermissions.View, UserPermissions.Manage,
                    ProjectPermissions.View, ProjectPermissions.Launch, ProjectPermissions.Update, ProjectPermissions.Delete,
                    TicketPermissions.View, TicketPermissions.Create, TicketPermissions.Update, TicketPermissions.Delete,
                    TicketPermissions.Assign, TicketPermissions.Unassign,
                }
            },
            {
                // プロジェクトに関する操作は Project Role に委譲
                SystemRole.RoleType.User, Array.Empty<string>()
            }
        };
}
