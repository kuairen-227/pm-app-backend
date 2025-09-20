using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Common.Authorization;

namespace WebApi.Application.Common.Authorization;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IPermissionService _permissionService;
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;

    public AuthorizationBehavior(IPermissionService permissionService, IUserContext userContext, IUserRepository userRepository)
    {
        _permissionService = permissionService;
        _userContext = userContext;
        _userRepository = userRepository;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var attributes = request.GetType()
            .GetCustomAttributes(typeof(RequiresPermissionAttribute), true)
            .Cast<RequiresPermissionAttribute>()
            .ToList();

        if (attributes.Any())
        {
            var user = await _userRepository.GetByIdAsync(_userContext.Id, cancellationToken)
                ?? throw new AuthenticationException("USER_NOT_FOUND", "ユーザーが存在しません");

            Guid? projectId = null;
            if (request is IProjectScopedRequest scoped)
            {
                projectId = scoped.ProjectId;
            }


            foreach (var attr in attributes)
            {
                await _permissionService.EnsurePermissionAsync(user, attr.PermissionCode, projectId, cancellationToken);
            }
        }

        return await next();
    }
}
