using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Common.Mapper;

public static class SystemRoleMapper
{
    public static SystemRole.RoleType? Map(string? role)
    {
        if (role is null)
            return null;

        if (!Enum.TryParse<SystemRole.RoleType>(role, true, out var value))
            throw new ApplicationException("INVALID_SYSTEM_ROLE", $"SystemRole '{role}' は無効です");

        return value;
    }
}
