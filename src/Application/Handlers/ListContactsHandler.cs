using ContactAgenda.Application.DTOs;
using ContactAgenda.Application.Interfaces;
using ContactAgenda.Application.Queries;
using MediatR;

namespace ContactAgenda.Application.Handlers;

/// <summary>
/// Handler for listing all contacts (optimized read with Dapper)
/// </summary>
public sealed class ListContactsHandler : IRequestHandler<ListContactsQuery, IReadOnlyList<ContactListDto>>
{
    private readonly IContactReadRepository _readRepository;

    public ListContactsHandler(IContactReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<IReadOnlyList<ContactListDto>> Handle(ListContactsQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.ListAllAsync(cancellationToken);
    }
}
