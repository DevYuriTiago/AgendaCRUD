using ContactAgenda.Application.Commands;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Interfaces;
using ContactAgenda.Domain.Entities;
using ContactAgenda.Domain.Repositories;
using MediatR;

namespace ContactAgenda.Application.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public RegisterHandler(IUserRepository userRepository, IRoleRepository roleRepository,
        IRefreshTokenRepository refreshTokenRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.UsernameExistsAsync(request.Username, cancellationToken))
            throw new ArgumentException("Username already exists");

        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
            throw new ArgumentException("Email already exists");

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = User.Create(request.Username, request.Email, passwordHash, request.FullName);
        await _userRepository.AddAsync(user, cancellationToken);

        var userRole = await _roleRepository.GetByNameAsync(Role.Names.User, cancellationToken) 
            ?? throw new InvalidOperationException("Default role not found");
        
        var userRoleEntity = UserRole.Create(user.Id, userRole.Id);
        await _userRepository.AddUserRoleAsync(userRoleEntity, cancellationToken);

        var roles = new[] { Role.Names.User };
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var refreshToken = await _tokenService.CreateRefreshTokenAsync(user.Id, "127.0.0.1");
        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        return new AuthResponse(
            user.Id, user.Username, user.Email, user.FullName, roles,
            accessToken, refreshToken.Token, refreshToken.ExpiresAt
        );
    }
}
