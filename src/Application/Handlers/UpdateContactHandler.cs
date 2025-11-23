using AutoMapper;
using ContactAgenda.Application.Commands;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Domain.Exceptions;
using ContactAgenda.Domain.Repositories;
using ContactAgenda.Domain.ValueObjects;
using MediatR;

namespace ContactAgenda.Application.Handlers;

/// <summary>
/// Handler for updating an existing contact
/// </summary>
public sealed class UpdateContactHandler : IRequestHandler<UpdateContactCommand, ContactDto>
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public UpdateContactHandler(IContactRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        // Get existing contact
        var contact = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ContactNotFoundException(request.Id);

        // Create value objects (validation happens here)
        var email = Email.Create(request.Email);
        var phone = PhoneNumber.Create(request.Phone);

        // Check email uniqueness (excluding current contact)
        if (await _repository.EmailExistsAsync(email, request.Id, cancellationToken))
            throw new DuplicateEmailException(email);

        // Update domain entity
        contact.Update(request.Name, email, phone);

        // Persist
        await _repository.UpdateAsync(contact, cancellationToken);

        return _mapper.Map<ContactDto>(contact);
    }
}
