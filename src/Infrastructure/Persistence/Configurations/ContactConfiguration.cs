using ContactAgenda.Domain.Entities;
using ContactAgenda.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactAgenda.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core entity configuration for Contact
/// </summary>
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Email value object mapping
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(254)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value))
            .HasColumnName("Email");

        // Phone value object mapping
        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(15)
            .HasConversion(
                phone => phone.Value,
                value => PhoneNumber.Create(value))
            .HasColumnName("Phone");

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt);

        // Unique constraint on email
        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Contacts_Email_Unique");

        // Index for performance
        builder.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Contacts_Name");
    }
}
