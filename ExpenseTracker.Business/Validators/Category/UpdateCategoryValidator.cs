using ExpenseTracker.Business.Dtos.Category;
using FluentValidation;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Kategori adı boş olamaz.")
            .MaximumLength(30).WithMessage("Kategori adı en fazla 30 karakter olabilir.");

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Açıklama en fazla 250 karakter olabilir.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
