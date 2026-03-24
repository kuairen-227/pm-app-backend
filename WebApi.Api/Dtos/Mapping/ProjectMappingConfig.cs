using Mapster;
using WebApi.Api.Dtos.Request.Projects;
using WebApi.Api.Dtos.Response.Projects;
using WebApi.Application.Commands.Projects.ChangeMemberRole;
using WebApi.Application.Commands.Projects.InviteMember;
using WebApi.Application.Commands.Projects.LaunchProject;
using WebApi.Application.Commands.Projects.UpdateProject;
using WebApi.Application.Common.Mapper;
using WebApi.Application.Queries.Projects.Dtos;

namespace WebApi.Api.Dtos.Mapping;

/// <summary>
/// Project Mapping
/// </summary>
public class ProjectMappingConfig : IRegister
{
    /// <summary>
    /// Mapping 登録
    /// </summary>
    public void Register(TypeAdapterConfig config)
    {
        // Request DTO → Command
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

        // Application DTO → Response DTO
        config.NewConfig<ProjectDto, ProjectResponse>();

        config.NewConfig<ProjectDetailDto, ProjectDetailResponse>();

        config.NewConfig<ProjectMemberDto, ProjectMemberResponse>();
    }
}
