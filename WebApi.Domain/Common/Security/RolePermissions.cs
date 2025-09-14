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
                    TicketPermissions.Create, TicketPermissions.Update, TicketPermissions.Delete
                }
            },
            {
                Role.RoleType.Manager, new []
                {
                    ProjectPermissions.Create, ProjectPermissions.Update, ProjectPermissions.Delete,
                    TicketPermissions.Create, TicketPermissions.Update, TicketPermissions.Delete
                }
            },
            {
                Role.RoleType.Member, new []
                {
                    // TODO: 1つ以上必要なため仮で設定
                    ProjectPermissions.Create
                }
            }
        };
}
