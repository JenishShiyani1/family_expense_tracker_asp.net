using System.ComponentModel.DataAnnotations;

namespace Family_Expense_Tracker.Models
{
    public class SavingModel
    {
        public int? SavingID { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Saving Name is required.")]
        [StringLength(100, ErrorMessage = "Saving Name cannot be more than 100 characters.")]
        public string SavingName { get; set; }

        [Required(ErrorMessage = "Targeted Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Targeted Amount must be greater than zero.")]
        public decimal TargetedAmount { get; set; }
        public decimal SavedAmount { get; set; }

        [Required(ErrorMessage = "Deadline is required.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        public DateOnly DeadLine { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class SavingContribution
    {
        public int ContributionID { get; set; }
        public int SavingID { get; set; }
        public string SavingName { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public DateOnly AddDate { get; set; }
    }
}
