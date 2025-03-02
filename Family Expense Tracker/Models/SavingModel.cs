namespace Family_Expense_Tracker.Models
{
    public class SavingModel
    {
        public int? SavingID { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }
        public string SavingName { get; set; }
        public decimal TargetedAmount { get; set; }
        public decimal SavedAmount { get; set; }
        public DateOnly DeadLine { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
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
