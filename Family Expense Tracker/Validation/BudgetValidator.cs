using Family_Expense_Tracker.Models;
using FluentValidation;

namespace Family_Expense_Tracker.Validation
{
    public class BudgetValidator : AbstractValidator<BudgetModel>
    {
        public BudgetValidator()
        {
            RuleFor(budget => budget.CategoryID)
                .GreaterThan(0).WithMessage("CategoryID must be greater than 0.");

            RuleFor(budget => budget.TotalBudget)
                .GreaterThan(0).WithMessage("TotalBudget must be greater than 0.")
                .ScalePrecision(2, 18).WithMessage("TotalBudget must have up to 2 decimal places.");

            RuleFor(budget => budget.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .Must(endDate => endDate >= DateOnly.FromDateTime(DateTime.Now));
        }
    }
}
