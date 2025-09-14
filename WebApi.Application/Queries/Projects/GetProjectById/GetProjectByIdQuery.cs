using MediatR;
using WebApi.Application.Queries.Projects.Dtos;

namespace WebApi.Application.Queries.Projects.GetProjectById;

public class GetProjectByIdQuery : IRequest<ProjectDto?>
{
    public Guid Id { get; }

    public GetProjectByIdQuery(Guid id)
    {
        Id = id;
    }
}
