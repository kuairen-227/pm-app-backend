using WebApi.Application.Helpers.Common;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Tests.Helpers;

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
