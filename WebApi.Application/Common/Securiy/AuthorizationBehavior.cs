using MediatR;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Common.Security;

namespace WebApi.Application.Common.Security;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;

    public AuthorizationBehavior(
        IAuthorizationService authorizationService, IUserContext userContext, IUserRepository userRepository)
    {
        _authorizationService = authorizationService;
        _userContext = userContext;
        _userRepository = userRepository;
    }

    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var attributes = request.GetType().GetCustomAttributes(typeof(RequiresPermissionAttribute), true)
            .Cast<RequiresPermissionAttribute>()
            .ToList();

        if (attributes.Any())
        {
            var user = await _userRepository.GetByIdAsync(_userContext.Id, cancellationToken)
                ?? throw new AuthenticationException("USER_NOT_FOUND", "ユーザーが存在しません");

            foreach (var attr in attributes)
            {
                var permission = Permission.Create(attr.PermissionCode);
                _authorizationService.EnsurePermission(user, permission);
            }
        }

        return await next();
    }
}
