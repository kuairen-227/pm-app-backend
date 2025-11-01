using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class ProjectMemberBuilder : BaseBuilder<ProjectMemberBuilder, ProjectMember>
{
    private Guid _userId = Guid.NewGuid();
    private ProjectRole _role = ProjectRole.Create(ProjectRole.RoleType.Member);

    public ProjectMemberBuilder WithUserId(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public ProjectMemberBuilder WithRole(ProjectRole.RoleType roleType)
    {
        _role = ProjectRole.Create(roleType);
        return this;
    }

    public override ProjectMember Build()
    {
        return new ProjectMember(
            _userId,
            _role,
            _createdBy,
            _clock
        );
    }
}
