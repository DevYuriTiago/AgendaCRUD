using System.Reflection;
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

        // MediatR
        services.AddMediatR(config => 
            config.RegisterServicesFromAssembly(assembly));

        // AutoMapper
        services.AddAutoMapper(assembly);

        return services;
    }
}
