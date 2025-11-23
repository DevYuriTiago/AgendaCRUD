using ContactAgenda.Application.Commands;
using FluentValidation;

namespace ContactAgenda.Application.Validators;

public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
    public UpdateContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID do contato é obrigatório");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome não pode exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório")
            .EmailAddress().WithMessage("E-mail deve ser um endereço de e-mail válido")
            .MaximumLength(255).WithMessage("E-mail não pode exceder 255 caracteres");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefone é obrigatório")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Telefone deve ser um número de telefone internacional válido");
    }
}
