namespace ContactAgenda.Application.Interfaces;

/// <summary>
/// Service interface for password hashing and verification
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
