using Family_Expense_Tracker.Models;
using FluentValidation;

namespace Family_Expense_Tracker.Validation
{
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.CategoryName)
            .NotEmpty().WithMessage("CategoryName is required.")
            .MaximumLength(50).WithMessage("CategoryName cannot exceed 50 characters.");

            RuleFor(category => category.IsIncomeCategory)
            .NotNull().WithMessage("IsIncomeCategory must be specified.");
        }
    }
}
