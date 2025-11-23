using ContactAgenda.Domain.Entities;
using ContactAgenda.Domain.Repositories;
using ContactAgenda.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactAgenda.Infrastructure.Repositories;

public class EfRoleRepository : IRoleRepository
{
    private readonly ContactAgendaDbContext _context;

    public EfRoleRepository(ContactAgendaDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Role role, CancellationToken cancellationToken = default)
    {
        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
