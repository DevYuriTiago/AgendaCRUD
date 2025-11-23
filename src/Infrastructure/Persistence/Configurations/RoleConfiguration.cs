using ContactAgenda.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactAgenda.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        builder.Property(r => r.Description).HasMaxLength(200);
        builder.HasIndex(r => r.Name).IsUnique();
        
        // Seed default roles
        builder.HasData(
            new { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = Role.Names.Admin, Description = "Full system access", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = Role.Names.User, Description = "Regular user access", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = Role.Names.Guest, Description = "Read-only access", CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
