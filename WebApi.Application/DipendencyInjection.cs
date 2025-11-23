using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Abstractions;
using WebApi.Application.Common;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Events.Projects.MemberRoleChanged;
using WebApi.Application.Events.Users.UserRegistered;
using WebApi.Domain.Aggregates.ProjectAggregate.Events;
using WebApi.Domain.Aggregates.UserAggregate.Events;
using WebApi.Domain.Common.Authorization;

namespace WebApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // AutoMapper
        services.AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly);

        // Permission Services
        services.AddScoped<IPermissionService, PermissionService>();

        // Domain Events
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<IDomainEventMapper<UserRegisteredEvent>, UserRegisteredEventMapper>();
        services.AddScoped<IDomainEventMapper<ProjectMemberInvitedEvent>, MemberInvitedEventMapper>();
        services.AddScoped<IDomainEventMapper<ProjectRoleChangedEvent>, MemberRoleChangedEventMapper>();

        return services;
    }
}
