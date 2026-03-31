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
        config.NewConfig<LaunchProjectRequest, LaunchProjectCommand>()
            .ConstructUsing(src => new LaunchProjectCommand(
                src.Name,
                src.Description
            ));

        config.NewConfig<(Guid projectId, UpdateProjectRequest request), UpdateProjectCommand>()
            .ConstructUsing(src => new UpdateProjectCommand(
                src.projectId,
                src.request.Name,
                src.request.Description
            ));

        config.NewConfig<(Guid projectId, InviteMemberRequest request), InviteMemberCommand>()
            .ConstructUsing(src => new InviteMemberCommand(
                src.projectId,
                src.request.UserId,
                ProjectRoleMapper.Map(src.request.ProjectRole)
            ));

        config.NewConfig<(Guid projectId, Guid userId, ChangeMemberRoleRequest request), ChangeMemberRoleCommand>()
            .ConstructUsing(src => new ChangeMemberRoleCommand(
                src.projectId,
                src.userId,
                ProjectRoleMapper.Map(src.request.ProjectRole)
            ));

        // Application DTO → Response DTO
        config.NewConfig<ProjectDto, ProjectResponse>();

        config.NewConfig<ProjectDetailDto, ProjectDetailResponse>();

        config.NewConfig<ProjectMemberDto, ProjectMemberResponse>();
    }
}
