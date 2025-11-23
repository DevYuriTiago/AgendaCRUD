using ContactAgenda.Domain.Entities;

namespace ContactAgenda.Application.Interfaces;

/// <summary>
/// Service interface for JWT token generation and validation
/// </summary>
public interface ITokenService
{
    string GenerateAccessToken(User user, IEnumerable<string> roles);
    string GenerateRefreshToken();
    Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, string ipAddress);
}
