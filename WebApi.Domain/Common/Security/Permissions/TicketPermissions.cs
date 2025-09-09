namespace WebApi.Domain.Common.Security.Permissions;

public static class TicketPermissions
{
    public static readonly Permission Raise = Permission.Create("TICKET_RAISE");
    public static readonly Permission Delete = Permission.Create("TICKET_DELETE");
}
