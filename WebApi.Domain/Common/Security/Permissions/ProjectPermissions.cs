namespace WebApi.Domain.Common.Security.Permissions;

public static class ProjectPermissions
{
    public static readonly Permission Create = Permission.Create("PROJECT_CREATE");
    public static readonly Permission Update = Permission.Create("PROJECT_UPDATE");
    public static readonly Permission Delete = Permission.Create("PROJECT_DELETE");
}
