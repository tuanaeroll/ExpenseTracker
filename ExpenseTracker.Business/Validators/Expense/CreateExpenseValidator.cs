using ExpenseTracker.Business.Dtos.Expense;
using FluentValidation;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequestDto>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık boş olamaz.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Tutar sıfırdan büyük olmalıdır.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0).WithMessage("Geçerli bir ödeme yöntemi seçilmelidir.");

        RuleFor(x => x.Description)
            .MaximumLength(300).WithMessage("Açıklama en fazla 300 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Receipt)
            .Must(file => file == null || file.Length <= 2 * 1024 * 1024)
            .WithMessage("Yüklenen dosya 2MB'den büyük olamaz.");
    }
}
