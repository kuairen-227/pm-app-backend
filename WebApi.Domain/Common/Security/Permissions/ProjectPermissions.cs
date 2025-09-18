namespace WebApi.Domain.Common.Security.Permissions;

public static class ProjectPermissions
{
    public static readonly Permission Launch = Permission.Create("PROJECT_LAUNCH");
    public static readonly Permission Update = Permission.Create("PROJECT_UPDATE");
    public static readonly Permission Delete = Permission.Create("PROJECT_DELETE");
}
