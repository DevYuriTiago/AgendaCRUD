namespace ContactAgenda.Domain.Entities;

/// <summary>
/// User aggregate root for authentication and authorization
/// </summary>
public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    // Navigation properties
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();

    private User() { } // EF Core

    public static User Create(string username, string email, string passwordHash, string fullName)
    {
        ValidateUsername(username);
        ValidateEmail(email);
        ValidateFullName(fullName);

        return new User
        {
            Id = Guid.NewGuid(),
            Username = username.Trim().ToLowerInvariant(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            FullName = fullName.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string fullName)
    {
        ValidateFullName(fullName);
        FullName = fullName.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be empty", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));

        if (username.Length < 3)
            throw new ArgumentException("Username must be at least 3 characters long", nameof(username));

        if (username.Length > 50)
            throw new ArgumentException("Username cannot exceed 50 characters", nameof(username));

        if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9_.-]+$"))
            throw new ArgumentException("Username can only contain letters, numbers, dots, hyphens and underscores", nameof(username));
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException($"Invalid email format: {email}", nameof(email));

        if (email.Length > 254)
            throw new ArgumentException("Email is too long (max 254 characters)", nameof(email));
    }

    private static void ValidateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));

        if (fullName.Length < 3)
            throw new ArgumentException("Full name must be at least 3 characters long", nameof(fullName));

        if (fullName.Length > 100)
            throw new ArgumentException("Full name cannot exceed 100 characters", nameof(fullName));
    }
}
