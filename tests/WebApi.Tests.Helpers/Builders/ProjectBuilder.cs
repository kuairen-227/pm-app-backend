using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Tests.Helpers.Builders.Common;

namespace WebApi.Tests.Helpers.Builders;

public class ProjectBuilder : BaseBuilder<ProjectBuilder, Project>
{
    private string _name = "デフォルトプロジェクト";
    private string? _description;
    private List<ProjectMember> _members = new();

    public ProjectBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ProjectBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public ProjectBuilder WithMembers(params ProjectMember[] members)
    {
        _members = members.ToList();
        return this;
    }

    public override Project Build()
    {
        var project = new Project(
            _name,
            _description,
            _createdBy,
            _clock
        );

        foreach (var member in _members)
        {
            project.AddMember(member.UserId, member.Role);
        }

        return project;
    }
}
