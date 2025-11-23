namespace ContactAgenda.Application.DTOs;

public record RegisterRequest(
    string Username,
    string Email,
    string Password,
    string FullName
);

public record LoginRequest(
    string Username,
    string Password
);

public record RefreshTokenRequest(
    string RefreshToken
);

public record AuthResponse(
    Guid UserId,
    string Username,
    string Email,
    string FullName,
    string[] Roles,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt
);
