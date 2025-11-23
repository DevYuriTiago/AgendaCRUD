using ContactAgenda.Domain.Entities;

namespace ContactAgenda.Domain.Repositories;

/// <summary>
/// Repository interface for RefreshToken operations
/// </summary>
public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task RevokeUserTokensAsync(Guid userId, CancellationToken cancellationToken = default);
}
