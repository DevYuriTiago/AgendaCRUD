using ContactAgenda.Application.DTOs;
using MediatR;

namespace ContactAgenda.Application.Commands;

/// <summary>
/// Command to create a new contact
/// </summary>
public sealed record CreateContactCommand(string Name, string Email, string Phone) 
    : IRequest<ContactDto>;
