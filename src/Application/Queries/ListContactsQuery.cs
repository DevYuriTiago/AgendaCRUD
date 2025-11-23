using ContactAgenda.Application.DTOs;
using MediatR;

namespace ContactAgenda.Application.Queries;

/// <summary>
/// Query to list all contacts (optimized with Dapper)
/// </summary>
public sealed record ListContactsQuery : IRequest<IReadOnlyList<ContactListDto>>;
