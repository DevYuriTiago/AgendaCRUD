namespace ContactAgenda.Application.DTOs;

/// <summary>
/// Lightweight contact list item (for Dapper queries)
/// </summary>
public sealed record ContactListDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
