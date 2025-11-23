using ContactAgenda.Application.Commands;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Interfaces;
using ContactAgenda.Domain.Repositories;
using MediatR;

namespace ContactAgenda.Application.Handlers;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid refresh token");

        if (!refreshToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token is not active");

        var user = refreshToken.User;
        if (!user.IsActive)
            throw new UnauthorizedAccessException("User account is inactive");

        // Revoke old token
        refreshToken.Revoke(request.IpAddress);
        await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

        // Generate new tokens
        var roles = await _userRepository.GetUserRolesAsync(user.Id, cancellationToken);
        var roleNames = roles.Select(r => r.Name).ToArray();

        var newAccessToken = _tokenService.GenerateAccessToken(user, roleNames);
        var newRefreshToken = await _tokenService.CreateRefreshTokenAsync(user.Id, request.IpAddress);
        
        refreshToken.Revoke(request.IpAddress, newRefreshToken.Token);
        await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
        await _refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);

        return new AuthResponse(
            user.Id, user.Username, user.Email, user.FullName, roleNames,
            newAccessToken, newRefreshToken.Token, newRefreshToken.ExpiresAt
        );
    }
}
