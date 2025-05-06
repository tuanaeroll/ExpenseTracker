using ExpenseTracker.Business.Dtos.Expense;
using FluentValidation;

public class ApproveExpenseValidator : AbstractValidator<ApproveExpenseDto>
{
    public ApproveExpenseValidator()
    {
        RuleFor(x => x.ExpenseId)
            .GreaterThan(0).WithMessage("Geçerli bir masraf ID’si belirtilmelidir.");
    }
}
