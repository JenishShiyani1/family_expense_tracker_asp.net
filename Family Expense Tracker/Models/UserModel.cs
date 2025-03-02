namespace Family_Expense_Tracker.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int FamilyGroupID { get; set; }
        public string GroupName { get; set; }
        public string Role { get; set; }
    }
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class AdminRegister
    {
        public int? UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? FamilyGroupID { get; set; }
        public string GroupName { get; set; }
    }

    public class AddMember
    {
        public int? UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? FamilyGroupID { get;set; }
    }
}
