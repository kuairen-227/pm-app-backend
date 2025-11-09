using MediatR;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Queries.Users.Dtos;

namespace WebApi.Application.Queries.Users.ListUsers;

[RequiresPermission(UserPermissions.View)]
public class ListUsersQuery : IRequest<IReadOnlyList<UserDto>>
{
    public ListUsersQuery()
    {
    }
}
