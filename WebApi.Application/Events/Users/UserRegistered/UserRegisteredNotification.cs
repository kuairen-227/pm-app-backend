using MediatR;

namespace WebApi.Application.Events.Users.UserRegistered;

public sealed class UserRegisteredNotification : INotification
{
    public Guid UserId { get; }
    public string UserName { get; }

    public UserRegisteredNotification(Guid userId, string userName)
    {
        UserId = userId;
        UserName = userName;
    }
}
