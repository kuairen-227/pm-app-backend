using MediatR;

namespace WebApi.Application.Commands.Projects.DeleteProject;

public class DeleteProjectCommand : IRequest<Unit>
{
    public Guid Id { get; }

    public DeleteProjectCommand(Guid id)
    {
        Id = id;
    }
}
