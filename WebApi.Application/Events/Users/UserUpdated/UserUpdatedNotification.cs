using MediatR;
using WebApi.Domain.Aggregates.UserAggregate;

namespace WebApi.Application.Events.Users.UserUpdated;

public sealed class UserUpdatedNotification : INotification
{
    public Guid UserId { get; }

    public UserUpdatedNotification(Guid userId)
    {
        UserId = userId;
    }
}
