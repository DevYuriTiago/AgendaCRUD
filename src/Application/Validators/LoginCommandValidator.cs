using ContactAgenda.Application.Commands;
using FluentValidation;

namespace ContactAgenda.Application.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");

        RuleFor(x => x.IpAddress)
            .NotEmpty().WithMessage("IP Address is required");
    }
}
