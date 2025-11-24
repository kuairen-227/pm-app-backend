using Mapster;
using WebApi.Application.Commands.Projects.ChangeMemberRole;
using WebApi.Application.Commands.Projects.InviteMember;
using WebApi.Application.Commands.Projects.LaunchProject;
using WebApi.Application.Commands.Projects.UpdateProject;
using WebApi.Application.Common.Mapper;

namespace WebApi.Api.Dtos.Projects;

/// <summary>
/// Project Mapping（DTO → Command）
/// </summary>
public class ApiMappingConfig : IRegister
{
    /// <summary>
    /// DTO → Command の Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LaunchProjectRequest, LaunchProjectCommand>();

        config.NewConfig<(Guid projectId, UpdateProjectRequest), UpdateProjectCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId);

        config.NewConfig<(Guid projectId, InviteMemberRequest request), InviteMemberCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.ProjectRole, src => ProjectRoleMapper.Map(src.request.ProjectRole));

        config.NewConfig<(Guid projectId, Guid userId, ChangeMemberRoleRequest request), ChangeMemberRoleCommand>()
            .Map(dest => dest.ProjectId, src => src.projectId)
            .Map(dest => dest.UserId, src => src.userId)
            .Map(dest => dest.ProjectRole, src => ProjectRoleMapper.Map(src.request.ProjectRole));
    }
}
