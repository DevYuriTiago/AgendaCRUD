using ContactAgenda.Application.DTOs;
using MediatR;

namespace ContactAgenda.Application.Commands;

/// <summary>
/// Command to update an existing contact
/// </summary>
public sealed record UpdateContactCommand(Guid Id, string Name, string Email, string Phone) 
    : IRequest<ContactDto>;
