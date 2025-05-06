using ExpenseTracker.Business.Dtos.User;
using FluentValidation;

public class LoginUserValidator : AbstractValidator<LoginRequestDto>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş olamaz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.");
    }
}
