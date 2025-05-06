using ExpenseTracker.Business.Dtos.Expense;
using FluentValidation;

public class RejectExpenseValidator : AbstractValidator<RejectExpenseDto>
{
    public RejectExpenseValidator()
    {
        RuleFor(x => x.ExpenseId)
            .GreaterThan(0).WithMessage("Geçerli bir masraf ID’si belirtilmelidir.");

        RuleFor(x => x.ResponseNote)
            .NotEmpty().WithMessage("Red sebebi girilmelidir.")
            .MaximumLength(300).WithMessage("Red açıklaması en fazla 300 karakter olabilir.");
    }
}
