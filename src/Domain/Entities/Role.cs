namespace ContactAgenda.Domain.Entities;

/// <summary>
/// Role entity for role-based access control
/// </summary>
public class Role
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    private Role() { } // EF Core

    public static Role Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty", nameof(name));

        if (name.Length > 50)
            throw new ArgumentException("Role name cannot exceed 50 characters", nameof(name));

        return new Role
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Predefined roles
    public static class Names
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Guest = "Guest";
    }
}
