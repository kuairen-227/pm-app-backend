using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WebApi.Application.Common;

namespace WebApi.Infrastructure.Contexts;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid id
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User
                ?? throw new UnauthorizedAccessException("UserContext が取得できません");
            var sub = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("ユーザーID が見つかりません");

            return Guid.Parse(sub);
        }
    }
}
