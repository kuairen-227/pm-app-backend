using MediatR;

namespace WebApi.Application.Queries.Projects.GetProjectById;

public class GetProjectByIdQuery : IRequest<ProjectDto?>
{
    public Guid Id { get; }

    public GetProjectByIdQuery(Guid id)
    {
        Id = id;
    }
}
