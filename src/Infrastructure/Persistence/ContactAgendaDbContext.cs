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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactAgendaDbContext).Assembly);
    }
}
