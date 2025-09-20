using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;

namespace WebApi.Infrastructure.Contexts;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid Id
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User
                ?? throw new AuthenticationException("HTTP_CONTEXT_NOT_EXIT", "UserContext が取得できません");
            var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new AuthenticationException("USER_ID_NOT_EXIT", "ユーザーID が見つかりません");

            return Guid.Parse(sub);
        }
    }
}
