using ContactAgenda.Application.Interfaces;
using ContactAgenda.Domain.Repositories;
using ContactAgenda.Infrastructure.Persistence;
using ContactAgenda.Infrastructure.Repositories;
using ContactAgenda.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactAgenda.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ContactAgendaDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ContactAgendaDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped<IContactRepository, EfContactRepository>();
        services.AddScoped<IContactReadRepository, DapperContactReadRepository>();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddScoped<IRoleRepository, EfRoleRepository>();
        services.AddScoped<IRefreshTokenRepository, EfRefreshTokenRepository>();

        // Services
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
