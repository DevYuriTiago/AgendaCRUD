namespace ContactAgenda.Application.DTOs;

/// <summary>
/// Request to create a new contact
/// </summary>
public sealed record CreateContactRequest
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}
