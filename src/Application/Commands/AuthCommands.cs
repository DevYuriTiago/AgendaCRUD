using ContactAgenda.Application.DTOs;
using MediatR;

namespace ContactAgenda.Application.Commands;

public record RegisterCommand(
    string Username,
    string Email,
    string Password,
    string FullName
) : IRequest<AuthResponse>;

public record LoginCommand(
    string Username,
    string Password,
    string IpAddress
) : IRequest<AuthResponse>;

public record RefreshTokenCommand(
    string RefreshToken,
    string IpAddress
) : IRequest<AuthResponse>;
