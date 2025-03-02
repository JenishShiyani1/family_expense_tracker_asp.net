namespace Family_Expense_Tracker.Models
{
    public class MemberAdminModel
    {
        public string FamilyGroupName { get; set; }
        public string AdminName { get; set; }
        public List<MemberModel> Members { get; set; }
    }
}
