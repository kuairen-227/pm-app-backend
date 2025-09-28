using MediatR;
using WebApi.Application.Abstractions;
using WebApi.Domain.Aggregates.UserAggregate.Events;

namespace WebApi.Application.Events.Users.UserRoleChanged;

public sealed class UserRoleChangedEventMapper : IDomainEventMapper<UserRoleChangedEvent>
{
    public INotification? Map(UserRoleChangedEvent e) =>
        new UserRoleChangedNotification(
            e.UserId,
            e.Role
        );
}
