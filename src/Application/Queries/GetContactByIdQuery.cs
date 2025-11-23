using ContactAgenda.Application.DTOs;
using MediatR;

namespace ContactAgenda.Application.Queries;

/// <summary>
/// Query to get a single contact by ID
/// </summary>
public sealed record GetContactByIdQuery(Guid Id) : IRequest<ContactDto?>;
