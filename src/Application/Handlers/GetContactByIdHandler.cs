using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Interfaces;
using ContactAgenda.Application.Queries;
using MediatR;

namespace ContactAgenda.Application.Handlers;

/// <summary>
/// Handler for getting a contact by ID (optimized read with Dapper)
/// </summary>
public sealed class GetContactByIdHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
{
    private readonly IContactReadRepository _readRepository;

    public GetContactByIdHandler(IContactReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
