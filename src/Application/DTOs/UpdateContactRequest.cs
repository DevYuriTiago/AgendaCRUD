namespace ContactAgenda.Application.DTOs;

/// <summary>
/// Request to update an existing contact
/// </summary>
public sealed record UpdateContactRequest
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}
