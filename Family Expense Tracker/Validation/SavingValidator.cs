using Family_Expense_Tracker.Models;
using FluentValidation;

namespace Family_Expense_Tracker.Validation
{
    public class SavingValidator : AbstractValidator<SavingModel>
    {
        public SavingValidator()
        {
            RuleFor(saving => saving.SavingName)
                .NotEmpty().WithMessage("SavingName is required.")
                .MaximumLength(100).WithMessage("SavingName cannot exceed 100 characters.");

            RuleFor(saving => saving.TargetedAmount)
                .GreaterThan(0).WithMessage("TargetedAmount must be greater than 0.")
                .ScalePrecision(2, 18).WithMessage("TargetedAmount must have up to 2 decimal places.");

            RuleFor(saving => saving.DeadLine)
                .NotEmpty().WithMessage("DeadLine is required.")
                .Must(deadline => deadline > DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("DeadLine must be a future date.");
        }
    }
}
