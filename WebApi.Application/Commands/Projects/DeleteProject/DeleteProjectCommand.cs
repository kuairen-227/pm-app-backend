using MediatR;

namespace WebApi.Application.Commands.Projects.DeleteProject;

public class DeleteProjectCommand : IRequest<Unit>
{
    public required Guid Id { get; set; }
}
