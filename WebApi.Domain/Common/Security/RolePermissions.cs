using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common.Security.Permissions;

namespace WebApi.Domain.Common.Security;

public static class RolePermissions
{
    public static readonly IReadOnlyDictionary<Role.RoleType, IReadOnlyCollection<Permission>> Map =
        new Dictionary<Role.RoleType, IReadOnlyCollection<Permission>>
        {
            {
                Role.RoleType.Admin, new []
                {
                    UserPermissions.Manage,
                    ProjectPermissions.Create, ProjectPermissions.Update, ProjectPermissions.Delete,
                    TicketPermissions.Raise, TicketPermissions.Delete
                }
            },
            {
                Role.RoleType.Manager, new []
                {
                    ProjectPermissions.Create, ProjectPermissions.Update, ProjectPermissions.Delete,
                    TicketPermissions.Raise, TicketPermissions.Delete
                }
            },
            {
                Role.RoleType.Member, new []
                {
                    ProjectPermissions.Create
                }
            }
        };
}
