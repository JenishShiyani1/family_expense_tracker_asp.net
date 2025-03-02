namespace Family_Expense_Tracker.Models
{
    public class BudgetModel
    {
        public int? BudgetID { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal? RemainingBudget { get; set; }
        public DateOnly EndDate { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class RemainingBudgetModel
    {
        public decimal? RemainingBudget { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
