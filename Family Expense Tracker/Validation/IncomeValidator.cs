using Family_Expense_Tracker.Models;
using FluentValidation;

namespace Family_Expense_Tracker.Validation
{
    public class IncomeValidator : AbstractValidator<IncomeModel>
    {
        public IncomeValidator()
        {

            RuleFor(income => income.CategoryID)
                .GreaterThan(0).WithMessage("CategoryID must be greater than 0.");

            RuleFor(income => income.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.")
                .ScalePrecision(2, 18).WithMessage("Amount must have up to 2 decimal places.");

            RuleFor(income => income.IncomeDate)
                .NotEmpty().WithMessage("IncomeDate is required.")
                .Must(incomeDate => incomeDate <= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("IncomeDate cannot be in the future.");

            RuleFor(income => income.Notes)
                .MaximumLength(250).WithMessage("Notes cannot exceed 250 characters.");
        }
    }
}
