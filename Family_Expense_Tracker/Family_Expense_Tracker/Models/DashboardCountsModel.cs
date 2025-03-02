namespace Family_Expense_Tracker.Models
{
    public class DashboardCountsModel
    {
        public string Metric { get; set; }
        public decimal Value { get; set; }
    }

    public class CategoryWiseExpense
    {
        public string CategoryName { get; set; }
        public decimal TotalExpense { get; set; }
    }

    public class FamilyMember
    {
        public string MemberName { get; set; }
        public string Role { get; set; }
    }

    public class FamilyAdmin
    {
        public string FamilyGroupName { get; set; }
        public string AdminName { get; set; }
    }

    public class RecentExpense
    {
        public int ExpenseID { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Notes { get; set; }
    }

    public class RecentIncome
    {
        public int IncomeID { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
        public string Notes { get; set; }
    }

    public class FinanceDashboardData
    {
        public List<DashboardCountsModel> Counts { get; set; }
        public List<CategoryWiseExpense> CategoryWiseExpenses { get; set; }
        public List<FamilyMember> FamilyMembers { get; set; }
        public List<FamilyAdmin> FamilyAdmins { get; set; }
        public List<RecentExpense> RecentExpenses { get; set; }
        public List<RecentIncome> RecentIncomes { get; set; }
    }


    public class AdminDashboardCountsModel
    {
        public string Metric { get; set; }
        public decimal Value { get; set; }
    }

    public class AdminCategoryWiseExpense
    {
        public string CategoryName { get; set; }
        public decimal TotalExpense { get; set; }
    }
    public class FamilyGroupName
    {
        public int FamilyGroupID { get; set; }
        public string GroupName { get; set; }
    }
    public class AdminSideMember
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GroupName { get; set; }
    }

    public class AdminRecentExpense
    {
        public int ExpenseID { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Notes { get; set; }
    }

    public class AdminRecentIncome
    {
        public int IncomeID { get; set; }
        public string CategoryName { get; set; }
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
        public string Notes { get; set; }
    }

    public class AdminFinanceDashboardData
    {
        public List<AdminDashboardCountsModel> Counts { get; set; }
        public List<AdminCategoryWiseExpense> CategoryWiseExpenses { get; set; }
        public FamilyGroupName FamilyGroupName { get; set; }
        public List<AdminSideMember> AdminSideMember { get; set; }
        public List<AdminRecentExpense> RecentExpenses { get; set; }
        public List<AdminRecentIncome> RecentIncomes { get; set; }
    }
}
