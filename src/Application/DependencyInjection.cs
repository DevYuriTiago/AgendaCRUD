using System.Reflection;
using ContactAgenda.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ContactAgenda.Application;

/// <summary>
/// Application layer dependency injection
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // MediatR with Pipeline Behaviors
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            
            // Register pipeline behaviors in order
            config.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(assembly);

        // AutoMapper
        services.AddAutoMapper(assembly);

        return services;
    }
}
