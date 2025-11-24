using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ContactAgenda.Infrastructure.Persistence;

namespace ContactAgenda.Integration.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ContactAgendaDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            services.AddDbContext<ContactAgendaDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
                options.EnableSensitiveDataLogging();
            });

            // Build service provider and seed database
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ContactAgendaDbContext>();
            
            db.Database.EnsureCreated();
            SeedDatabase(db);
        });
    }

    private static bool _seeded = false;
    private static readonly object _lock = new object();

    private static void SeedDatabase(ContactAgendaDbContext context)
    {
        lock (_lock)
        {
            if (_seeded) return;

            if (!context.Roles.Any())
            {
                var adminRole = Domain.Entities.Role.Create("Admin", "Full system access");
                var userRole = Domain.Entities.Role.Create("User", "Regular user access");
                var guestRole = Domain.Entities.Role.Create("Guest", "Read-only access");

                context.Roles.AddRange(adminRole, userRole, guestRole);
                context.SaveChanges();
            }

            _seeded = true;
        }
    }
}

