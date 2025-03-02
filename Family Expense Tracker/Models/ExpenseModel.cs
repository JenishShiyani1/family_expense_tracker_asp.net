namespace Family_Expense_Tracker.Models
{
    public class ExpenseModel
    {
        public int? ExpenseID { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public int? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateOnly ExpenseDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
