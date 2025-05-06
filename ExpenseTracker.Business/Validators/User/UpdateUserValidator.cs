using ExpenseTracker.Business.Dtos.User;
using FluentValidation;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserValidator()
    {
        When(x => x.FirstName != null, () =>
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad alanı boş olamaz.")
                .MaximumLength(20).WithMessage("Ad en fazla 20 karakter olabilir.");
        });

        When(x => x.LastName != null, () =>
        {
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
                .MaximumLength(20).WithMessage("Soyad en fazla 20 karakter olabilir.");
        });

        When(x => x.PhoneNumber != null, () =>
        {
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10}$")
                .WithMessage("Telefon numarası 10 haneli olmalıdır. (örn: 5XXXXXXXXX)");
        });

        When(x => x.Position != null, () =>
        {
            RuleFor(x => x.Position)
                .MaximumLength(50).WithMessage("Pozisyon en fazla 50 karakter olabilir.");
        });
    }
}
