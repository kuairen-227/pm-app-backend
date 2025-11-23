using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Common.Mapper;

public static class ProjectRoleMapper
{
    public static ProjectRole.RoleType Map(string role)
    {
        if (!Enum.TryParse<ProjectRole.RoleType>(role, true, out var value))
            throw new ApplicationException("INVALID_PROJECT_ROLE", $"ProjectRole '{role}' は無効です");

        return value;
    }
}
