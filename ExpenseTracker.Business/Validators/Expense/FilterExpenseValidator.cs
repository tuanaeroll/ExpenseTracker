using ExpenseTracker.Business.Dtos.Expense;
using FluentValidation;

public class FilterExpenseValidator : AbstractValidator<FilterExpenseRequestDto>
{
    public FilterExpenseValidator()
    {
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).When(x => x.CategoryId.HasValue)
            .WithMessage("Kategori ID geçersiz.");

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0).When(x => x.PaymentMethodId.HasValue)
            .WithMessage("Ödeme yöntemi ID geçersiz.");

        RuleFor(x => x.CreatedAt)
            .LessThanOrEqualTo(DateTime.Now).When(x => x.CreatedAt.HasValue)
            .WithMessage("Oluşturulma tarihi bugünden ileri olamaz.");

        RuleFor(x => x.PaidAt)
            .LessThanOrEqualTo(DateTime.Now).When(x => x.PaidAt.HasValue)
            .WithMessage("Ödeme tarihi gelecekte olamaz.");
    }
}
