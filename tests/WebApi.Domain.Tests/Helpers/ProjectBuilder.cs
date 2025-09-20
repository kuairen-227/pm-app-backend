using WebApi.Domain.Aggregates.ProjectAggregate;
using WebApi.Domain.Helpers.Common;

namespace WebApi.Domain.Tests.Helpers;

public class ProjectBuilder : BaseBuilder<ProjectBuilder, Project>
{
    private string _name = "デフォルトプロジェクト";
    private string? _description;

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

    public override Project Build()
    {
        return new Project(
            _name,
            _description,
            _createdBy,
            _clock
        );
    }
}
