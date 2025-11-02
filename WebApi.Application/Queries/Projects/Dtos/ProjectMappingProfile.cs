using AutoMapper;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Queries.Projects.Dtos;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectDto>();

        CreateMap<ProjectMember, ProjectMemberDto>();
    }
}
