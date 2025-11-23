using ContactAgenda.Domain.ValueObjects;

namespace ContactAgenda.Domain.Entities;

/// <summary>
/// Contact aggregate root with domain invariants
/// </summary>
public class Contact
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public PhoneNumber Phone { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF Core constructor
    private Contact() { }

    private Contact(Guid id, string name, Email email, PhoneNumber phone)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        CreatedAt = DateTime.UtcNow;
    }

    public static Contact Create(string name, Email email, PhoneNumber phone)
    {
        ValidateName(name);
        
        return new Contact(Guid.NewGuid(), name, email, phone);
    }

    public void Update(string name, Email email, PhoneNumber phone)
    {
        ValidateName(name);

        Name = name;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        var trimmedName = name.Trim();

        if (trimmedName.Length < 3)
            throw new ArgumentException("Name must have at least 3 characters", nameof(name));

        if (trimmedName.Length > 100)
            throw new ArgumentException("Name is too long (max 100 characters)", nameof(name));
    }
}
