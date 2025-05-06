using ExpenseTracker.Business.Dtos.User;
using FluentValidation;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequestDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş olamaz.")
            .MaximumLength(20).WithMessage("Ad en fazla 20 karakter olabilir.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(20).WithMessage("Ad en fazla 20 karakter olabilir.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
            .MaximumLength(20).WithMessage("Soyad en fazla 20 karakter olabilir.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Telefon numarası boş olamaz.")
            .Matches(@"^\d{10}$").WithMessage("Telefon numarası 10 haneli olmalıdır. (örn: 5XXXXXXXXX)");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

        RuleFor(x => x.Position)
            .MaximumLength(50).WithMessage("Pozisyon en fazla 50 karakter olabilir.");

        RuleFor(x => x.IBAN)
    .NotEmpty().WithMessage("IBAN alanı boş olamaz.")
    .Length(26).WithMessage("IBAN tam olarak 26 karakter olmalıdır.")
    .Matches(@"^TR\d{24}$").WithMessage("IBAN 'TR' ile başlamalı ve ardından 24 rakam olmalıdır.");


    }
}
