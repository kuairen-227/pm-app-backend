namespace WebApi.Application.Common.Authorization;

public interface IProjectScopedRequest
{
    Guid ProjectId { get; }
}
