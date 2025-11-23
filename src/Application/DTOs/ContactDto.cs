namespace ContactAgenda.Application.DTOs;

/// <summary>
/// Contact data transfer object
/// </summary>
public sealed record ContactDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
