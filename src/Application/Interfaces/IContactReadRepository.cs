using ContactAgenda.Application.DTOs;

namespace ContactAgenda.Application.Interfaces;

/// <summary>
/// Read-only repository for optimized queries (Dapper implementation)
/// </summary>
public interface IContactReadRepository
{
    Task<ContactDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContactListDto>> ListAllAsync(CancellationToken cancellationToken = default);
}
