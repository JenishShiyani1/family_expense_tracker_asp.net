namespace Family_Expense_Tracker.Models
{
    public class CategoryModel
    {
        public int? CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool IsIncomeCategory { get; set; }
        public int? FamilyGroupID { get; set; }
        public int? UserID { get; set; }
        public string? UserName { get; set; }
    }

    public class CategoryDropDown
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
