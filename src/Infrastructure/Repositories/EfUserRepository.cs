using ContactAgenda.Domain.Entities;
using ContactAgenda.Domain.Repositories;
using ContactAgenda.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContactAgenda.Infrastructure.Repositories;

/// <summary>
/// EF Core repository for User write operations
/// </summary>
public class EfUserRepository : IUserRepository
{
    private readonly ContactAgendaDbContext _context;

    public EfUserRepository(ContactAgendaDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();
        
        // Use EF Core LINQ instead of raw SQL for compatibility with InMemoryDatabase
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username.ToLower() == normalizedUsername, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        
        // Use EF Core LINQ instead of raw SQL for compatibility with InMemoryDatabase
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim().ToLowerInvariant();
        
        // Use EF Core LINQ instead of raw SQL for compatibility with InMemoryDatabase
        return await _context.Users
            .AnyAsync(u => u.Username.ToLower() == normalizedUsername, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        
        // Use EF Core LINQ instead of raw SQL for compatibility with InMemoryDatabase
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task AddUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        await _context.UserRoles.AddAsync(userRole, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
