using WebApi.Application.Helpers.Common;
using WebApi.Domain.Aggregates.ProjectAggregate;

namespace WebApi.Application.Tests.Helpers;

public class ProjectBuilder : BaseBuilder<ProjectBuilder, Project>
{
    private string _name = "デフォルトプロジェクト";
    private string? _description;
    private Guid _ownerId = Guid.NewGuid();

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

    public ProjectBuilder WithOwnerId(Guid ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public override Project Build()
    {
        return new Project(
            _name,
            _description,
            _ownerId,
            _createdBy,
            _clock
        );
    }
}
