using ContactAgenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactAgenda.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for Contact Agenda
/// </summary>
public class ContactAgendaDbContext : DbContext
{
    public ContactAgendaDbContext(DbContextOptions<ContactAgendaDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactAgendaDbContext).Assembly);
    }
}
