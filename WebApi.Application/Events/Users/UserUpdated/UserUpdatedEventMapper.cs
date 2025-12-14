using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Domain.Aggregates.UserAggregate.Events;

namespace WebApi.Application.Events.Users.UserUpdated;

public sealed class UserUpdatedEventMapper : IDomainEventMapper<UserUpdatedEvent>
{
    public INotification? Map(UserUpdatedEvent e) =>
        new UserUpdatedNotification(e.UserId);
}
