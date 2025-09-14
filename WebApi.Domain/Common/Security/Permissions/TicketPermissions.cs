namespace WebApi.Domain.Common.Security.Permissions;

public static class TicketPermissions
{
    public static readonly Permission Create = Permission.Create("TICKET_CREATE");
    public static readonly Permission Update = Permission.Create("TICKET_UPDATE");
    public static readonly Permission Delete = Permission.Create("TICKET_DELETE");
}
