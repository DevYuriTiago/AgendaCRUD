using AutoMapper;
using ContactAgenda.Application.Commands;
using ContactAgenda.Application.DTOs;
using ContactAgenda.Domain.Entities;
using ContactAgenda.Domain.Exceptions;
using ContactAgenda.Domain.Repositories;
using ContactAgenda.Domain.ValueObjects;
using MediatR;

namespace ContactAgenda.Application.Handlers;

/// <summary>
/// Handler for creating a new contact
/// </summary>
public sealed class CreateContactHandler : IRequestHandler<CreateContactCommand, ContactDto>
{
    private readonly IContactRepository _repository;
    private readonly IMapper _mapper;

    public CreateContactHandler(IContactRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        // Create value objects (validation happens here)
        var email = Email.Create(request.Email);
        var phone = PhoneNumber.Create(request.Phone);

        // Check email uniqueness
        if (await _repository.EmailExistsAsync(email, null, cancellationToken))
            throw new DuplicateEmailException(email);

        // Create domain entity
        var contact = Contact.Create(request.Name, email, phone);

        // Persist
        await _repository.AddAsync(contact, cancellationToken);

        return _mapper.Map<ContactDto>(contact);
    }
}
