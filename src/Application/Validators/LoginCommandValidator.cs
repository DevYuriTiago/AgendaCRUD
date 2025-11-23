using ContactAgenda.Application.Commands;
using FluentValidation;

namespace ContactAgenda.Application.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória");

        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("Endereço IP é obrigatório");
    }
}
