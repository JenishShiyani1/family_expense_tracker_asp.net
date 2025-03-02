namespace Family_Expense_Tracker.Models
{
    public class MemberModel
    {
        public int MemberID { get; set; }
        public string FamilyGroupName { get; set; }
        public int MemberUserID { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
