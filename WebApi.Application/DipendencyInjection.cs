using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Application.Events.Projects.MemberRoleChanged;
using WebApi.Application.Events.Users.UserRegistered;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;
using WebApi.Domain.Aggregates.UserAggregate.Events;

namespace WebApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Domain Events
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<IDomainEventMapper<UserRegisteredEvent>, UserRegisteredEventMapper>();
        services.AddScoped<IDomainEventMapper<ProjectMemberInvitedEvent>, MemberInvitedEventMapper>();
        services.AddScoped<IDomainEventMapper<ProjectRoleChangedEvent>, MemberRoleChangedEventMapper>();

        return services;
    }
}
