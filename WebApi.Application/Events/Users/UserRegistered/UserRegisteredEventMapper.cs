using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Domain.Aggregates.UserAggregate.Events;

namespace WebApi.Application.Events.Users.UserRegistered;

public sealed class UserRegisteredEventMapper : IDomainEventMapper<UserRegisteredEvent>
{
    public INotification? Map(UserRegisteredEvent e) =>
        new UserRegisteredNotification(
            e.UserId,
            e.UserName
        );
}
