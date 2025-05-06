using ExpenseTracker.Business.Dtos.User;
using FluentValidation;

public class ChangeCredentialsRequestValidator : AbstractValidator<ChangeCredentialsRequestDto>
{
    public ChangeCredentialsRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Mevcut şifre boş olamaz.");

        RuleFor(x => x.NewPassword)
            .MinimumLength(6).When(x => !string.IsNullOrWhiteSpace(x.NewPassword))
            .WithMessage("Yeni şifre en az 6 karakter olmalıdır.");

        RuleFor(x => x.NewEmail)
            .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.NewEmail));
    }
}
