using ExpenseTracker.Business.Dtos.PaymentMethod;
using FluentValidation;

public class UpdatePaymentMethodValidator : AbstractValidator<UpdatePaymentMethodDto>
{
    public UpdatePaymentMethodValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Ödeme yöntemi adı boş olamaz.")
            .MaximumLength(100).WithMessage("Ödeme yöntemi adı en fazla 100 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
