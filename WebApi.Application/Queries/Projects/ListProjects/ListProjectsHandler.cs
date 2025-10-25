using AutoMapper;
using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Application.Common.Authorization.Permissions;
using WebApi.Application.Queries.Projects.Dtos;
using WebApi.Domain.Abstractions.Repositories;
using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Aggregates.UserAggregate;
using WebApi.Domain.Common.Authorization;

namespace WebApi.Application.Queries.Projects.ListProjects;

public class ListProjectsHandler : IRequestHandler<ListProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IPermissionService _permissionService;
    private readonly IMapper _mapper;

    public ListProjectsHandler(
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IUserContext userContext,
        IPermissionService permissionService,
        IMapper mapper
    )
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _userContext = userContext;
        _permissionService = permissionService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> Handle(ListProjectsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(_userContext.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(User), _userContext.Id);

        var canViewAll = await _permissionService.HasPermissionAsync(user, ProjectPermissions.ViewAll, null, cancellationToken);

        IEnumerable<Project> projects;
        if (canViewAll)
        {
            projects = await _projectRepository.ListAllAsync(cancellationToken);
        }
        else
        {
            projects = await _projectRepository.ListByUserIdAsync(user.Id, cancellationToken);
        }

        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }
}
