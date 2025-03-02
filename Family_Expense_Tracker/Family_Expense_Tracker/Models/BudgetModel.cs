using System.ComponentModel.DataAnnotations;

namespace Family_Expense_Tracker.Models
{
    public class BudgetModel
    {
        public int? BudgetID { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Total Budget is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Total Budget must be greater than zero.")]
        public decimal TotalBudget { get; set; }
        public decimal? RemainingBudget { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        public DateOnly EndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class RemainingBudgetModel
    {
        public decimal? RemainingBudget { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
