using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Application.Abstractions;
using WebApi.Application.Abstractions.AuthService;
using WebApi.Application.Commands.Tickets.UpdateTicket;
using WebApi.Application.Common;
using WebApi.Application.Common.Authorization;
using WebApi.Application.Events.Projects.MemberRoleChanged;
using WebApi.Application.Events.Users.UserRegistered;
using WebApi.Application.Services;
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
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggingBehavior<,>));

        // AutoMapper
        services.AddAutoMapper(cfg => { }, typeof(DependencyInjection).Assembly);

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<UpdateTicketService>();

        // Domain Events
        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddScoped<IDomainEventMapper<UserRegisteredEvent>, UserRegisteredEventMapper>();
        services.AddScoped<IDomainEventMapper<ProjectMemberInvitedEvent>, MemberInvitedEventMapper>();
        services.AddScoped<IDomainEventMapper<ProjectRoleChangedEvent>, MemberRoleChangedEventMapper>();

        return services;
    }
}
