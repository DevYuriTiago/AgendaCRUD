using ContactAgenda.Application.Commands;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Interfaces;
using ContactAgenda.Domain.Repositories;
using MediatR;

namespace ContactAgenda.Application.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken)
            ?? throw new UnauthorizedAccessException("Nome de usuário ou senha inválidos");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Conta de usuário está inativa");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Nome de usuário ou senha inválidos");

        var roles = await _userRepository.GetUserRolesAsync(user.Id, cancellationToken);
        var roleNames = roles.Select(r => r.Name).ToArray();

        var accessToken = _tokenService.GenerateAccessToken(user, roleNames);
        var refreshToken = await _tokenService.CreateRefreshTokenAsync(user.Id, request.IpAddress);
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        return new AuthResponse(
            user.Id, user.Username, user.Email, user.FullName, roleNames,
            accessToken, refreshToken.Token, refreshToken.ExpiresAt
        );
    }
}
