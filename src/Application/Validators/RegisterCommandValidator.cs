using ContactAgenda.Application.Commands;
using FluentValidation;

namespace ContactAgenda.Application.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório")
            .Length(3, 50).WithMessage("Nome de usuário deve ter entre 3 e 50 caracteres")
            .Matches(@"^[a-zA-Z0-9._-]+$")
            .WithMessage("Nome de usuário pode conter apenas caracteres alfanuméricos, pontos, hífens e sublinhados");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório")
            .EmailAddress().WithMessage("E-mail deve ser um endereço de e-mail válido")
            .MaximumLength(254).WithMessage("E-mail não pode exceder 254 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres")
            .Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula")
            .Matches(@"[0-9]").WithMessage("Senha deve conter pelo menos um número")
            .Matches(@"[@$!%*?&#]").WithMessage("Senha deve conter pelo menos um caractere especial (@$!%*?&#)");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Nome completo é obrigatório")
            .Length(3, 100).WithMessage("Nome completo deve ter entre 3 e 100 caracteres");
    }
}
