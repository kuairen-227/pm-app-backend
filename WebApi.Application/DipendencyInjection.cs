using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;
using WebApi.Domain.Aggregates.UserAggregate.Events;

namespace WebApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();

        // User Event Mappers
        services.AddScoped<IDomainEventMapper<UserRegisteredEvent>, UserRegisteredEventMapper>();

        // Project Event Mappers
        services.AddScoped<IDomainEventMapper<ProjectMemberInvitedEvent>, MemberInvitedEventMapper>();
        services.AddScoped<IDomainEventMapper<ProjectRoleChangedEvent>, RoleChangedEventMapper>();

        return services;
    }
}
