using ContactAgenda.Application.Commands;
using ContactAgenda.Domain.Exceptions;
using ContactAgenda.Domain.Repositories;
using MediatR;

namespace ContactAgenda.Application.Handlers;

/// <summary>
/// Handler for deleting a contact
/// </summary>
public sealed class DeleteContactHandler : IRequestHandler<DeleteContactCommand, Unit>
{
    private readonly IContactRepository _repository;

    public DeleteContactHandler(IContactRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ContactNotFoundException(request.Id);

        await _repository.DeleteAsync(contact, cancellationToken);

        return Unit.Value;
    }
}
