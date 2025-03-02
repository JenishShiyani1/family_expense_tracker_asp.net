using Family_Expense_Tracker.Models;
using FluentValidation;

namespace Family_Expense_Tracker.Validation
{
    public class ExpenseValidator : AbstractValidator<ExpenseModel>
    {
        public ExpenseValidator()
        {
            RuleFor(expense => expense.CategoryID)
                .GreaterThan(0).WithMessage("CategoryID must be greater than 0.");

            RuleFor(expense => expense.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.")
                .ScalePrecision(2, 18).WithMessage("Amount must have up to 2 decimal places.");

            RuleFor(expense => expense.ExpenseDate)
                .NotEmpty().WithMessage("ExpenseDate is required.")
                .Must(expenseDate => expenseDate <= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("ExpenseDate cannot be in the future.");

            RuleFor(expense => expense.Notes)
                .MaximumLength(250).WithMessage("Notes cannot exceed 250 characters.");
        }
    }
}
