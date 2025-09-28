using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Application.Events.Users.UserRegistered;
using WebApi.Domain.Aggregates.UserAggregate.Events;

public sealed class UserRegisteredEventMapper : IDomainEventMapper<UserRegisteredEvent>
{
    public INotification? Map(UserRegisteredEvent e) =>
        new UserRegisteredNotification(
            e.UserId,
            e.UserName
        );
}
