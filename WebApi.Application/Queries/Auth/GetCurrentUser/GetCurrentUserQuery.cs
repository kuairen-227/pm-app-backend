using MediatR;
using WebApi.Application.Queries.Users.Dtos;

namespace WebApi.Application.Queries.Auth.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserDto>
{
    public GetCurrentUserQuery()
    {
    }
}
