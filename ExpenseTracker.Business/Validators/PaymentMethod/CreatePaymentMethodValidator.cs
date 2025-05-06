using ExpenseTracker.Business.Dtos.PaymentMethod;
using FluentValidation;

public class CreatePaymentMethodValidator : AbstractValidator<CreatePaymentMethodDto>
{
    public CreatePaymentMethodValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ödeme yöntemi adı boş olamaz.")
            .MaximumLength(50).WithMessage("Ödeme yöntemi adı en fazla 50 karakter olabilir.");

        RuleFor(x => x.Description)
               .MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olabilir.")
               .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
