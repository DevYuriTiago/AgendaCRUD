using ContactAgenda.Domain.Entities;
using ContactAgenda.Domain.Repositories;
using ContactAgenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace ContactAgenda.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for Contact write operations
/// </summary>
public class EfContactRepository : IContactRepository
{
    private readonly ContactAgendaDbContext _context;

    public EfContactRepository(ContactAgendaDbContext context)
    {
        _context = context;
    }

    public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Contact?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        
        // Use FromSqlRaw to query directly against the stored string column
        return await _context.Contacts
            .FromSqlRaw("SELECT * FROM Contacts WHERE Email = {0}", normalizedEmail)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeContactId = null, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        
        // For SQL Server: Use raw SQL because EF Core cannot translate Value Object properties
        // For InMemory (tests): Use LINQ which works fine
        if (_context.Database.IsRelational())
        {
            // Raw SQL for production (SQL Server)
            if (excludeContactId.HasValue)
            {
                return await _context.Contacts
                    .FromSqlRaw("SELECT * FROM Contacts WHERE LOWER(Email) = {0} AND Id != {1}", normalizedEmail, excludeContactId.Value)
                    .AnyAsync(cancellationToken);
            }
            else
            {
                return await _context.Contacts
                    .FromSqlRaw("SELECT * FROM Contacts WHERE LOWER(Email) = {0}", normalizedEmail)
                    .AnyAsync(cancellationToken);
            }
        }
        else
        {
            // LINQ for InMemory (tests)
            if (excludeContactId.HasValue)
            {
                return await _context.Contacts
                    .AnyAsync(c => c.Email.Value.ToLower() == normalizedEmail && c.Id != excludeContactId.Value, cancellationToken);
            }
            else
            {
                return await _context.Contacts
                    .AnyAsync(c => c.Email.Value.ToLower() == normalizedEmail, cancellationToken);
            }
        }
    }

    public async Task AddAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        await _context.Contacts.AddAsync(contact, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        _context.Contacts.Update(contact);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
